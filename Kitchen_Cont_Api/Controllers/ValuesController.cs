using Kitchen_Cont_Api.Entities;
using Kitchen_Cont_Api.Helper;
using Kitchen_Cont_Api.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kitchen_Cont_Api.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {

        #region Properties

        private TCPHelper _TCPHelper;
        private TCPHelper oTCPHelper
        {
            get
            {
                if (_TCPHelper == null)
                    _TCPHelper = new TCPHelper();
                return _TCPHelper;
            }
        }

        #endregion

        //[BasicAuthentication]
        [Route("GetValues")]
        [HttpGet]
        public IEnumerable<string> GetValues()
        {
            //Dictionary<string, string> _dict = new Dictionary<string, string>();
            //_dict.Add("1", "Abc");
            //_dict.Add("2", "xyz");

            //string k = _dict.FirstOrDefault(x => x.Value == "Abc").Key;
            return new string[] { "value1", "value2" };
        }
        [Route("GetTest")]
        [HttpGet]
        public ResponseInfo GetTest()
        {
            ResponseInfo info = new ResponseInfo();
            info.Result = 1;
            info.Msg = "Success";
            info.DeviceResponse = "Success";

            return info;
        }
        [BasicAuthentication]
        [Route("SetScheduleTime")]
        [HttpPost]
        public ResponseInfo SetScheduleTime(DeviceScheduleSetTimeInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            try
            {
                string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                Constants.SEPARATOR +
                                Constants.HEADER +
                                Constants.SEPARATOR +
                                (int)Enums.PacketTypeInfo.Set_Time +
                                Constants.SEPARATOR +
                                dinfo.ChannelNo +
                                Constants.SEPARATOR +
                                dinfo.SchNo +
                                Constants.SEPARATOR +
                                dinfo.StartTimeHH +
                                Constants.SEPARATOR +
                                dinfo.StartTimeMM +
                                Constants.SEPARATOR +
                                dinfo.EndTimeHH +
                                Constants.SEPARATOR +
                                dinfo.EndTimeMM +
                                Constants.SEPARATOR +
                                Constants.END_BYTE;
                info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
        [BasicAuthentication]
        [Route("GetScheduleTime")]
        [HttpPost]
        public ResponseInfo GetScheduleTime(DeviceScheduleGetTimeInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Get_Time +
                                   Constants.SEPARATOR +
                                   dinfo.ChannelNo +
                                   Constants.SEPARATOR +
                                   dinfo.SchNo +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;
                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
        [BasicAuthentication]
        [Route("OutputStatusInfo")]
        [HttpPost]
        public ResponseInfo OutputStatusInfo(OutputStatusInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Output_Status +
                                   Constants.SEPARATOR +
                                   dinfo.ChannelNo +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;
                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
        [BasicAuthentication]
        [Route("GetDateTimeOfDeviceInfo")]
        [HttpPost]
        public ResponseInfo GetDateTimeOfDeviceInfo(DeviceGetDateTimeInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Get_Date_Time_Device +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;
                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
        [BasicAuthentication]
        [Route("GetOutputControlInfo")]
        [HttpPost]
        public ResponseInfo GetOutputControlInfo(DeviceOutputControlInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Ouput_Control_Packet +
                                   Constants.SEPARATOR +
                                   dinfo.ChannelNo +
                                   Constants.SEPARATOR +
                                   dinfo.State +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;
                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
        [BasicAuthentication]
        [Route("SchedulerTimeExtensionInfo")]
        [HttpPost]
        public ResponseInfo SchedulerTimeExtensionInfo(DeviceSchedulerTimeExtInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Scheduler_Time_Extension +
                                   Constants.SEPARATOR +
                                   dinfo.ChannelNo +
                                   Constants.SEPARATOR +
                                   dinfo.Alert +
                                   Constants.SEPARATOR +
                                   dinfo.TimeHH +
                                   Constants.SEPARATOR +
                                   dinfo.TimeMM +
                                   Constants.SEPARATOR +
                                   dinfo.TimeSS +
                                   Constants.SEPARATOR +
                                   dinfo.DateDD +
                                   Constants.SEPARATOR +
                                   dinfo.DateMM +
                                   Constants.SEPARATOR +
                                   dinfo.DateYY +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;

                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
        [BasicAuthentication]
        [Route("SetTimeAndDate")]
        [HttpPost]
        public ResponseInfo SetTimeAndDate(SetDateAndTimeDeviceInfo dinfo)
        {
            ResponseInfo info = new ResponseInfo();
            if (ModelState.IsValid)
            {

                try
                {
                    string TCP_IP = ConfigurationManager.AppSettings["TCP_IP"];
                    int TCP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["TCP_PORT"]);
                    string Input = dinfo.ImeiNo + Constants.IMEI_APPEND +
                                   Constants.SEPARATOR +
                                   Constants.HEADER +
                                   Constants.SEPARATOR +
                                   (int)Enums.PacketTypeInfo.Set_Time_Date +
                                   Constants.SEPARATOR +
                                   dinfo.TimeHH +
                                   Constants.SEPARATOR +
                                   dinfo.TimeMM +
                                   Constants.SEPARATOR +
                                   dinfo.TimeSS +
                                   Constants.SEPARATOR +
                                   dinfo.DateDD +
                                   Constants.SEPARATOR +
                                   dinfo.DateMM +
                                   Constants.SEPARATOR +
                                   dinfo.DateYY +
                                   Constants.SEPARATOR +
                                   Constants.END_BYTE;
                    info = oTCPHelper.Connect(TCP_IP, TCP_PORT, Input, Constants.GET_SUCCESS);
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
            info.Result = 0;
            info.Msg = Constants.INPUT_REQUIRED;
            info.DeviceResponse = Constants.INPUT_REQUIRED;

            return info;
        }
    }
}
