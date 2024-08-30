using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace AJ_KITCHEN_SENS_SVC.Common
{
    public class CLDevices
    {
        public static Dictionary<string, string> DIC_CONNECT = new Dictionary<string, string>();
        public static Dictionary<string, TcpClient> DIC_CLIENT_CONNECT = new Dictionary<string, TcpClient>();
    }
}
