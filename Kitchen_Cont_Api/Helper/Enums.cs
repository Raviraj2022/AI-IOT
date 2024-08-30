using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kitchen_Cont_Api.Helper
{
    public class Enums
    {
        public enum PacketTypeInfo
        {

            Set_Time = 101,
            Get_Time = 201,
            Output_Status = 102,
            Get_Date_Time_Device = 205,
            Ouput_Control_Packet = 103,
            Scheduler_Time_Extension = 104,
            Set_Time_Date = 105
        }
    }
}