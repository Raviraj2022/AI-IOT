using AJ_KITCHEN_SENS_SVC.Common;
using AJ_KITCHEN_SENS_SVC.Entities;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using NHAI_AIS140_Server.Common;

namespace AJ_KITCHEN_SENS_SVC
{
    class Program
    {
        #region Properties
        private static OperationContext _OperationContext;
        private static readonly object padlock = new object();
        private static OperationContext oOperationContext
        {
            get
            {
                lock (padlock)
                {
                    if (_OperationContext == null)
                        _OperationContext = new OperationContext();
                    return _OperationContext;
                }
            }
        }
        public static TcpListener server = null;

        public static List<TcpClient> connectedClients = null;
        public static GSettings settings = null;
        public static string IMEI { get; set; }
        public static List<DeviceDataInfo> devicedatalst = null;
        #endregion
        static async Task Main(string[] args)
        {
            LoadJson();
            connectedClients = new List<TcpClient>();
            devicedatalst = new List<DeviceDataInfo>();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 50 * 1000;//50 sec;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, settings.Port));

            Console.WriteLine("Listening on port - " + settings.Port);

            listenSocket.Listen(120);

            try
            {


                while (true)
                {

                    var socket = await listenSocket.AcceptAsync();

                    _ = ProcessLinesAsync(socket);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //
            //
        }
        public static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CloseAndRemoveAllClientConnect();
        }
        public static void CloseAndRemoveAllClientConnect()
        {
            try
            {

                //foreach (TcpClient tinfo in connectedClients)
                //{
                //    if (!tinfo.Connected)
                //    {
                //        connectedClients.Remove(tinfo);

                //        if (CLDevices.DIC_CONNECT.ContainsKey(tinfo.Client.RemoteEndPoint.ToString()))
                //            CLDevices.DIC_CONNECT.Remove(tinfo.Client.RemoteEndPoint.ToString());
                //    }
                //}
                DateTime TDate = DateTime.Now;
                if (devicedatalst.Count > 0)
                {
                    List<DeviceDataInfo> dlst = devicedatalst.Where(i => TDate.Subtract(i.SyncOn).TotalSeconds >= CommonHelper.Device_Clear_Seconds).ToList();
                    foreach (DeviceDataInfo info in dlst)
                    {
                        devicedatalst.Remove(info);
                    }

                }

                Dictionary<string, TcpClient> _dict = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Value.Client == null).ToDictionary(i => i.Key, i => i.Value);
                foreach (var info in _dict)
                {
                    try
                    {
                        info.Value.GetStream().Close();
                        info.Value.Client.Close();
                    }
                    catch (Exception ex)
                    {

                    }

                    CLDevices.DIC_CLIENT_CONNECT.Remove(info.Key);

                    if (CLDevices.DIC_CONNECT.ContainsKey(info.Key))
                        CLDevices.DIC_CONNECT.Remove(info.Key);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static async Task ProcessLinesAsync(Socket socket)
        {
            Console.WriteLine($"[{socket.RemoteEndPoint}]: connected");

            // Create a PipeReader over the network stream
            var stream = new NetworkStream(socket);
            var reader = PipeReader.Create(stream);

            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                {

                    foreach (var segment in line)
                    {
                        //TcpClient client = new TcpClient();
                        //client.Client = socket;
                        //connectedClients.Add(client);

                        string rcvData = Encoding.UTF8.GetString(segment);
                        StringBuilder kdataS = new StringBuilder(CommonHelper.BytesToHex(segment.ToArray(), false));// ParseHelper.ByteArrayToString(byteBuffer, size);

                        string HexVal = kdataS.ToString();
                        if (!string.IsNullOrEmpty(rcvData))
                        {
                            ResponseInfo DResult = null;

                            ValidatePacket(rcvData, HexVal, socket);
                        }
                    }
                }

                // Tell the PipeReader how much of the buffer has been consumed.
                reader.AdvanceTo(buffer.Start, buffer.End);

                // Stop reading if there's no more data coming.
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Mark the PipeReader as complete.
            await reader.CompleteAsync();

            Dictionary<string, string> _dict = CLDevices.DIC_CONNECT.Where(i => i.Value == socket.RemoteEndPoint.ToString()).ToDictionary(i => i.Key, i => i.Value);
            foreach (var info in _dict)
            {
                CLDevices.DIC_CONNECT.Remove(info.Key);
            }


            Dictionary<string, TcpClient> lst = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Value.Client.RemoteEndPoint.ToString() == socket.RemoteEndPoint.ToString()).ToDictionary(i => i.Key, i => i.Value);
            foreach (var info in lst)
            {
                try
                {
                    info.Value.GetStream().Close();
                    info.Value.Client.Close();
                }
                catch (Exception ex)
                {

                }

                CLDevices.DIC_CLIENT_CONNECT.Remove(info.Key);
            }


            //List<TcpClient> allclient = connectedClients.Where(i => i.Client.RemoteEndPoint.ToString() == socket.RemoteEndPoint.ToString()).ToList();
            //foreach (TcpClient tinfo in allclient)
            //{
            //    tinfo.GetStream().Close();
            //    tinfo.Close();
            //}
            Console.WriteLine($"[{socket.RemoteEndPoint}]: disconnected");
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            // Look for a EOL in the buffer.
            SequencePosition? position = buffer.PositionOf((byte)'#');

            if (position == null)
            {
                line = default;
                return false;
            }

            // Skip the line + the \n.
            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        public static void SetAndResetConnection(Socket socket, string ImeiNo, int IsLogin)
        {
            try
            {

                Dictionary<string, TcpClient> lst = new Dictionary<string, TcpClient>();
                if (IsLogin == 0)
                {
                    //if (CLDevices.DIC_CLIENT_CONNECT.ContainsKey(socket.RemoteEndPoint.ToString()))
                    //{
                    //    CLDevices.DIC_CONNECT[socket.RemoteEndPoint.ToString()] = IMEI;
                    //    Console.WriteLine("Connected Device-" + socket.RemoteEndPoint.ToString() + " Imei No-" + IMEI);
                    //}
                    Console.WriteLine("Connected Device-" + socket.RemoteEndPoint.ToString() + " Imei No-" + ImeiNo);
                    if (CLDevices.DIC_CLIENT_CONNECT.ContainsKey(ImeiNo))
                    {
                        lst = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Key == ImeiNo).ToDictionary(i => i.Key, i => i.Value);
                        foreach (var info in lst)
                        {
                            try
                            {
                                info.Value.GetStream().Close();
                                info.Value.Close();
                            }
                            catch (Exception ex)
                            {

                            }

                            CLDevices.DIC_CLIENT_CONNECT.Remove(info.Key);
                        }
                        TcpClient client = new TcpClient();
                        client.Client = socket;
                        CLDevices.DIC_CLIENT_CONNECT.Add(ImeiNo, client);
                    }
                    else
                    {
                        TcpClient client = new TcpClient();
                        client.Client = socket;
                        CLDevices.DIC_CLIENT_CONNECT.Add(ImeiNo, client);
                    }

                    if (CLDevices.DIC_CONNECT.ContainsKey(ImeiNo))
                    {
                        CLDevices.DIC_CONNECT.Remove(ImeiNo);

                        CLDevices.DIC_CONNECT.Add(ImeiNo, socket.RemoteEndPoint.ToString());
                    }
                    else
                    {
                        CLDevices.DIC_CONNECT.Add(ImeiNo, socket.RemoteEndPoint.ToString());
                    }

                }
                else if (IsLogin == 1)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        //private static void ProcessLine(in ReadOnlySequence<byte> buffer, Socket socket)
        //{
        //    foreach (var segment in buffer)
        //    {

        //        string rcvData = Encoding.UTF8.GetString(segment);

        //        if (!rcvData.Contains("LGN"))
        //        {
        //            Console.WriteLine("Data Packet " + rcvData);
        //            oOperationContext.DataSyncByString(rcvData);
        //        }
        //        else
        //        {
        //           Console.WriteLine("Login Packet " + rcvData);
        //            socket.Send(Encoding.ASCII.GetBytes("$LGN" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + "*"));
        //        }


        //    }
        //}
        public static void ValidatePacket(string Inputtxt, string HexVal, Socket socket)
        {
            string Response = "2";
            string PType = "";
            try
            {


                ///<summary>Login Packet Information</summary>
                if (ParseHelper.IsLogin(Inputtxt))
                {
                    LoginInfo loginpacket = ParseHelper.CastListToLoginInfo(Inputtxt);
                    if (loginpacket != null)
                    {
                        if (1 == 1)
                        {
                            Response = "1";
                            IMEI = loginpacket.ImeiNo;//ParseHelper.GetIMEI(dataS);

                            SetAndResetConnection(socket, IMEI, 0);

                            // Process the data sent by the client.
                            string dataSend = "@$," + loginpacket.PacketType + "," + Response + ",#";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(dataSend);
                            socket.Send(msg);
                        }

                        Console.WriteLine("Login Msg Received End Point-" + socket.RemoteEndPoint.ToString() + " Imei No-" + IMEI);

                    }


                }
                ///<summary>End Login Packet Information</summary>
                ///<summary>Start General Packet Information</summary>
                else if (ParseHelper.IsGeneralData(Inputtxt))
                {
                    Response = "201";

                    GeneralDataInfo generalpacket = ParseHelper.CastListToGeneralInfo(Inputtxt, HexVal);

                    if (generalpacket != null)
                    {
                        PType = generalpacket.PacketType;
                        // Console.WriteLine("General Packet Received ");

                        if (socket != null)
                        {
                            Console.WriteLine("General Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + IMEI);
                            if (CLDevices.DIC_CONNECT.ContainsValue(socket.RemoteEndPoint.ToString()))
                            {
                                Response = "101";
                                generalpacket.ImeiNo = CLDevices.DIC_CONNECT.FirstOrDefault(x => x.Value == socket.RemoteEndPoint.ToString()).Key;//CLDevices.DIC_CONNECT[socket.RemoteEndPoint.ToString()];
                                IMEI = generalpacket.ImeiNo;
                                if (!string.IsNullOrEmpty(generalpacket.ImeiNo))
                                    Task.Run(() => oOperationContext.PushGeneralPacketData(generalpacket, CommonHelper.IndianStandard(DateTime.UtcNow)));
                            }
                        }
                        else
                            Response = "201";
                    }
                    else
                    {
                        Console.WriteLine("General blank-");
                    }

                    string dataSend = string.Empty;
                    if (Response == "101")
                        dataSend = "@$," + generalpacket.PacketType + "," + generalpacket.PacketNumber + "," + Response + ",#";
                    else
                        dataSend = "@$,N/A,N/A" + "," + Response + ",#";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(dataSend);

                    // Send back a response.
                    socket.Send(msg);
                }
                ///<summary>End General Packet Information</summary>
                ///<summary>Start Heart Beat Packet Information</summary>
                else if (ParseHelper.IsHearBeatData(Inputtxt))
                {
                    HeartBeatDataInfo generalpacket = ParseHelper.CastListToHeartBeatInfo(Inputtxt);
                    //int Response = 2;

                    if (generalpacket != null)
                    {
                        Console.WriteLine("Heartbeat Packet Received End Point-" + socket.RemoteEndPoint.ToString() + " Imei No-" + IMEI);
                        if (CLDevices.DIC_CONNECT.ContainsValue(socket.RemoteEndPoint.ToString()))
                        {
                            Response = "1";
                        }
                    }

                    string dataSend = string.Empty;
                    if (Response == "1")
                        dataSend = "@$," + generalpacket.PacketType + "," + generalpacket.PacketNumber + "," + Response + ",#";
                    else
                        dataSend = "@$,N/A,N/A" + "," + Response + ",#";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(dataSend);

                    // Send back a response.
                    socket.Send(msg);
                }
                ///<summary>End Heart Beat Packet Information</summary>
                #region Set Schedule Time
                else if (ParseHelper.IsSetScheduleTimeData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Set Schedule Time Packet Received End Point-");

                        Console.WriteLine("Set Schedule Time Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Get Schedule Time
                else if (ParseHelper.IsGetScheduleTimeData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Get Schedule Time Packet Received End Point-");
                        Console.WriteLine("Get Schedule Time Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Output Status
                else if (ParseHelper.IsOutputStatusData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Output Status Packet Received End Point-");
                        Console.WriteLine("Output Status Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Get Date Time Of Device
                else if (ParseHelper.IsDateTimeDeviceData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Get Date Time Of Device Packet Received End Point-");
                        Console.WriteLine("Get Date Time Of Device Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Get Output Control
                else if (ParseHelper.IsOutputControlData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Output Control Packet Received End Point-");
                        Console.WriteLine("Output Control Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Get Schedule Time Extension
                else if (ParseHelper.IsScheduleTimeExtData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Schedule Time Extension Packet Received End Point-");
                        Console.WriteLine("Schedule Time Extension Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
                #region Set Time Date
                else if (ParseHelper.IsSetTimeDateData(Inputtxt))
                {
                    if (Inputtxt.Split(',').ToList()[0].Contains("_A"))
                    {
                        string ApiImeiNo = Inputtxt.Split(',').ToList()[0].Replace("_A", string.Empty);
                        Response = "Something Wrong Happened";
                        Response = SendApiData(Inputtxt, socket, "Set Time Date Packet Received End Point-");
                        Console.WriteLine("Set Time Date Packet Received End Point-" + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Response -" + Response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        // Send back a response.
                        socket.Send(msg);
                    }
                    else
                    {
                        GetOtherTimoutData(Inputtxt, socket);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Console.WriteLine("Ex line-" + line);
                if (PType == "2")
                {
                    try
                    {
                        string dataSend = "@$,N/A,N/A" + "," + Response + ",#";
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(dataSend);

                        // Send back a response.
                        socket.Send(msg);
                    }
                    catch (Exception exm)
                    {

                    }
                }
                CloseConnById(socket.RemoteEndPoint.ToString());
                socket.Close();
            }
        }
        public static void CloseConnById(string ConnectID)
        {
            lock (CLDevices.DIC_CONNECT)
            {
                try
                {
                    //if (CLDevices.DIC_CONNECT.ContainsValue(ConnectID))
                    //    CLDevices.DIC_CONNECT.Remove(ConnectID);

                    //List<TcpClient> allclient = connectedClients.Where(i => i.Client.RemoteEndPoint.ToString() == ConnectID).ToList();
                    //foreach (TcpClient tinfo in allclient)
                    //{
                    //    tinfo.GetStream().Close();
                    //    tinfo.Close();
                    //}

                    if (CLDevices.DIC_CONNECT.ContainsValue(ConnectID))
                    {
                        Dictionary<string, string> _dict = CLDevices.DIC_CONNECT.Where(i => i.Value == ConnectID).ToDictionary(i => i.Key, i => i.Value);
                        foreach (var info in _dict)
                        {
                            CLDevices.DIC_CONNECT.Remove(info.Key);
                        }
                    }



                    Dictionary<string, TcpClient> lst = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Value.Client.RemoteEndPoint.ToString() == ConnectID).ToDictionary(i => i.Key, i => i.Value);
                    foreach (var info in lst)
                    {
                        try
                        {
                            info.Value.GetStream().Close();
                            info.Value.Client.Close();
                        }
                        catch (Exception ex)
                        {

                        }

                        CLDevices.DIC_CLIENT_CONNECT.Remove(info.Key);
                    }
                }
                catch
                {
                }
            }
        }
        public static void LoadJson()
        {
            using (StreamReader r = new StreamReader("GSettings.json"))
            {
                string json = r.ReadToEnd();
                GSettings items = JsonConvert.DeserializeObject<GSettings>(json);
                settings = items;
            }
        }
        public static void CloseConnByImei(string ConnectID)
        {
            Dictionary<string, string> _dict = CLDevices.DIC_CONNECT.Where(i => i.Key == ConnectID).ToDictionary(i => i.Key, i => i.Value);
            foreach (var info in _dict)
            {
                CLDevices.DIC_CONNECT.Remove(info.Key);
            }


            Dictionary<string, TcpClient> lst = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Key == ConnectID).ToDictionary(i => i.Key, i => i.Value);
            foreach (var info in lst)
            {
                try
                {
                    info.Value.GetStream().Close();
                    info.Value.Client.Close();
                }
                catch (Exception ex)
                {

                }

                CLDevices.DIC_CLIENT_CONNECT.Remove(info.Key);
            }
        }
        //public static void CloseConnect()
        //{
        //    try
        //    {
        //        if (this.sConn == null)
        //            return;
        //        this.IsReConnect = false;
        //        this.IsStartReceive = false;
        //        this.sConn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.WriteDebugMsg("TcpConnect-CloseConnect() Exception:" + ex.Message);
        //    }
        //}
        public static string SendApiData(string Inputtxt, Socket socket, string DataType)
        {
            string Response = string.Empty;
            try
            {
                List<string> _lstdata = Inputtxt.Split(',').ToList();
                string ApiImeiNo = _lstdata[0].Replace("_A", string.Empty);
                if (!string.IsNullOrEmpty(ApiImeiNo))
                {
                    _lstdata.RemoveAt(0);
                    string Inputdata = String.Join(",", _lstdata);
                    Console.WriteLine(DataType + socket.RemoteEndPoint + " Imei No-" + ApiImeiNo + " Input-" + Inputtxt);
                    if (CLDevices.DIC_CONNECT.ContainsKey(ApiImeiNo))
                        Response = ValidateServerToDevice(Inputdata, ApiImeiNo);
                    else
                        Response = "This Device is not connected";
                }
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
        public static string ValidateServerToDevice(string InputTxt, string ImeiNo)
        {
            InputTxt = InputTxt + "#";
            DateTime TDate = DateTime.Now;
            string EndPoint = string.Empty;
            int timeOut = CommonHelper.Device_Recieve_Timeout_Seconds;//10000; //10 second timeout
            string Result = string.Empty;
            int IsValid = 0;
            List<string> Inputlst = InputTxt.Split(',').ToList();
            int IPacketType = Convert.ToInt32(Inputlst[1]);

            //    if (Responselst[2] != Inputlst[1] && Responselst[1] != "1")
            try
            {

                EndPoint = CLDevices.DIC_CONNECT.FirstOrDefault(i => i.Key == ImeiNo).Value;//
                TcpClient clientinfo = CLDevices.DIC_CLIENT_CONNECT.Where(i => i.Value.Client != null && i.Value.Client.RemoteEndPoint.ToString() == EndPoint).Select(i => new TcpClient()
                {
                    Client = i.Value.Client
                }).FirstOrDefault(); //connectedClients.Where(i => i.Client.RemoteEndPoint.ToString() == EndPoint).FirstOrDefault();
                if (clientinfo != null)
                {

                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(InputTxt);
                    //clientinfo.Client.ReceiveTimeout = timeOut;
                    clientinfo.Client.Send(data);
                    IsValid = 1;

                    System.Threading.Thread.Sleep(timeOut);
                    DeviceDataInfo dinfo = null;
                    if (IPacketType == ((int)Enums.PacketTypeInfo.Set_Time) || IPacketType == ((int)Enums.PacketTypeInfo.Get_Time))
                    {
                        int IChannelno = Convert.ToInt32(Inputlst[2]);
                        if (IChannelno < CommonHelper.Min_Channel_No || IChannelno > CommonHelper.Max_Channel_No)
                        {
                            dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.SchNo == Inputlst[3]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                            if (dinfo == null)
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        }
                        else
                        {
                            dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2] && i.SchNo == Inputlst[3]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                            if (dinfo == null)
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        }
                    }
                    else if (IPacketType == ((int)Enums.PacketTypeInfo.Get_Date_Time_Device) || IPacketType == ((int)Enums.PacketTypeInfo.Set_Time_Date))
                        dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                    else
                    {
                        int IChannelno = Convert.ToInt32(Inputlst[2]);
                        if (IChannelno < CommonHelper.Min_Channel_No || IChannelno > CommonHelper.Max_Channel_No)
                            dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        else
                            dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                    }
                    if (dinfo != null)
                    {
                        if (dinfo.Data.Contains("#"))
                            Result = dinfo.Data;
                        else
                            Result = dinfo.Data + "##";
                    }
                    else
                        Result = "Not Recieved Response from device. Timeout";
                    //var buffer = new byte[1024];
                    //Int32 bytes = clientinfo.Client.Receive(buffer);
                    //Result = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                    //if (!string.IsNullOrEmpty(Result))
                    //{
                    //    List<string> Responselst = Result.Split(',').ToList();
                    //    List<string> Inputlst = InputTxt.Split(',').ToList();
                    //    if (Responselst[2] != Inputlst[1] && Responselst[1] != "1")
                    //        Result = "Device Sent Wrong Response";
                    //}
                    //else
                    //{
                    //    CloseConnByImei(ImeiNo);
                    //    Result = "Recieved Blank Response From Device. If recieved same than disconnect and resend login packet again";
                    //}
                }
                else
                {
                    CloseConnByImei(ImeiNo);
                    Result = "Device is not connected. Please resend login packet than try";
                }
            }
            catch (SocketException ex)
            {
                switch (ex.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                        CloseConnByImei(ImeiNo);
                        Result = "Device is not connected. Please resend login packet than try";
                        break;
                    case 0:
                        CloseConnByImei(ImeiNo);
                        Result = "Device is not connected. Please resend login packet than try";
                        break;
                    case SocketError.TimedOut:
                        //DeviceDataInfo dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        DeviceDataInfo dinfo = null;
                        if (IPacketType == ((int)Enums.PacketTypeInfo.Set_Time) || IPacketType == ((int)Enums.PacketTypeInfo.Get_Time))
                        {
                            int IChannelno = Convert.ToInt32(Inputlst[2]);
                            if (IChannelno < CommonHelper.Min_Channel_No || IChannelno > CommonHelper.Max_Channel_No)
                            {
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.SchNo == Inputlst[3]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                                if (dinfo == null)
                                    dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                            }
                            else
                            {
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2] && i.SchNo == Inputlst[3]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                                if (dinfo == null)
                                    dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                            }
                        }
                        else if (IPacketType == ((int)Enums.PacketTypeInfo.Get_Date_Time_Device))
                            dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        else
                        {
                            int IChannelno = Convert.ToInt32(Inputlst[2]);
                            if (IChannelno < CommonHelper.Min_Channel_No || IChannelno > CommonHelper.Max_Channel_No)
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                            else
                                dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds && i.PacketType == Inputlst[1] && i.ChannelNumber == Inputlst[2]).OrderByDescending(i => i.SyncOn).FirstOrDefault();
                        }
                        if (dinfo != null)
                        {
                            if (dinfo.Data.Contains("#"))
                                Result = dinfo.Data;
                            else
                                Result = dinfo.Data + "##";
                        }
                        else
                            Result = "Not Recieved Response from device. Timeout";
                        break;
                    default:
                        CloseConnByImei(ImeiNo);
                        Result = "Something wrong happend";
                        break;
                }
                //switch (IsValid)
                //{
                //    case 0:
                //        CloseConnByImei(ImeiNo);
                //        Result = "Device is not connected. Please resend login packet than try";
                //        break;
                //    case 1:
                //        DeviceDataInfo dinfo = devicedatalst.Where(i => i.RemoteEndPoint == EndPoint && TDate.Subtract(i.SyncOn).TotalSeconds < CommonHelper.Device_Clear_Seconds).FirstOrDefault();
                //        if (dinfo != null)
                //        {
                //            if(dinfo.Data.Contains("#"))
                //            Result = dinfo.Data;
                //            else
                //                Result = dinfo.Data+"##";
                //        }
                //        else
                //            Result = "Not Recieved Response from device. Timeout";
                //        break;
                //    default:
                //        Result = "Something wrong happend";
                //        break;
                //}
                if (string.IsNullOrEmpty(Result))
                {
                    CloseConnByImei(ImeiNo);
                    Result = "Something wrong happend";
                }
                return Result;
            }
            if (string.IsNullOrEmpty(Result))
            {
                Console.WriteLine("line 2-" + IsValid);
                CloseConnByImei(ImeiNo);
                Result = "Something wrong happend";
            }
            return Result;
        }

        public static void GetOtherTimoutData(string Inputtxt, Socket socket)
        {
            try
            {
                List<string> _lstdata = Inputtxt.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    int IsApi = Convert.ToInt32(_lstdata[1]);
                    if (IsApi == 1)
                    {
                        int IPacketType = Convert.ToInt32(_lstdata[2]);
                        string IChannelNumber = "0";//_lstdata[3];
                        string SchNo = "0";
                        switch (IPacketType)
                        {
                            case ((int)Enums.PacketTypeInfo.Set_Time):
                                IsApi = Convert.ToInt32(_lstdata[1]);
                                IChannelNumber = _lstdata[3];
                                SchNo = _lstdata[4];
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Get_Time):
                                IsApi = Convert.ToInt32(_lstdata[1]);
                                IChannelNumber = _lstdata[3];
                                SchNo = _lstdata[4];
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Output_Status):
                                IsApi = Convert.ToInt32(_lstdata[1]);
                                IChannelNumber = _lstdata[3];
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Get_Date_Time_Device):
                                //IsApi = Convert.ToInt32(_lstdata[1]);
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Ouput_Control_Packet):
                                IsApi = Convert.ToInt32(_lstdata[1]);
                                IChannelNumber = _lstdata[3];
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Scheduler_Time_Extension):
                                IsApi = Convert.ToInt32(_lstdata[1]);
                                IChannelNumber = _lstdata[3];
                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                            case ((int)Enums.PacketTypeInfo.Set_Time_Date):

                                if (!string.IsNullOrEmpty(Inputtxt))
                                    devicedatalst.Add(new DeviceDataInfo { RemoteEndPoint = socket.RemoteEndPoint.ToString(), PacketType = IPacketType.ToString(), ChannelNumber = IChannelNumber, SchNo = SchNo, Data = Inputtxt.Trim(), SyncOn = DateTime.Now });
                                break;
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
