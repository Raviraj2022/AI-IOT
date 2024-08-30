using Kitchen_Cont_Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Kitchen_Cont_Api.Helper
{
    public class TCPHelper
    {
        public ResponseInfo SendDataOnTcp(string ServerIP, int Port, string InputMsg, string ResponseMsg)
        {
            ResponseInfo info = new ResponseInfo();
            try
            {
                //new Thread(() =>
                //{
                //    Thread.CurrentThread.IsBackground = true;
                //    string Msg = "";//binfo.IPower + "|" + info.Imei + Seperator + binfo.EngineNo + Seperator + info.Speed + Seperator + info.Lat + Seperator + info.Lng + Seperator + info.Odo + Seperator + info.Ignition + Seperator + info.IPower + Seperator + TDate.ToString("yyyy-MM-dd'T'HH:mm:ss") + Seperator + "VWSPL";
                //    //Connect("122.176.104.240", Msg);
                //    Connect(ServerIP, Port, Msg);
                //}).Start();
                Connect(ServerIP, Port, InputMsg, ResponseMsg);
            }
            catch (Exception ex)
            {
                info.Result = 0;
                info.Msg = Constants.FAILURE;
                info.DeviceResponse = string.Empty;

                return info;
            }
            return info;
        }
        public ResponseInfo Connect(String server, int Port, String InputMessage, string ResponseMsg)
        {
            ResponseInfo info = new ResponseInfo();
            try
            {
                //Int32 port = 1000;//3001;
                TcpClient client = new TcpClient(server, Port);
                NetworkStream stream = client.GetStream();
                int count = 0;
                //while (count++ < 3)
                //{
                // Translate the Message into ASCII.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(InputMessage);
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                // Console.WriteLine("Sent: {0}", message);
                // Bytes Array to receive Server Response.
                data = new Byte[1024];
                String response = String.Empty;
                // Read the Tcp Server Response Bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                // Console.WriteLine("Received: {0}", response);
                Thread.Sleep(200);
                //}
                stream.Close();
                client.Close();

                info.Result = 1;
                info.Msg = ResponseMsg;
                info.DeviceResponse = response;
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e);
                info.Result = 0;
                info.Msg = Constants.FAILURE;
                info.DeviceResponse = Constants.DEVICE_NOT_CONNECTED;

                return info;
            }
            //Console.Read();
            return info;
        }
    }
}