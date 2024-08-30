using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kitchen_Cont_Api.Entities
{
    public class ResponseInfo
    {
        public int Result { get; set; }
        public string Msg { get; set; }
        public string DeviceResponse { get; set; }
    }
}