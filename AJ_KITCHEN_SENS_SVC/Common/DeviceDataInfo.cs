using System;
using System.Collections.Generic;
using System.Text;

namespace AJ_KITCHEN_SENS_SVC.Common
{
    internal class DeviceDataInfo
    {
        public string RemoteEndPoint { get; set; }
        public string PacketType { get; set; }
        public string ChannelNumber { get; set; }
        public string  SchNo { get; set; } 
        public string Data { get; set; }
        public DateTime SyncOn { get; set; }
    }
}
