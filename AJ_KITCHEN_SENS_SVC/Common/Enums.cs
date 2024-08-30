using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHAI_AIS140_Server.Common
{
    internal class Enums
    {
        public enum TicketStatus
        {
            OPEN = 0,
            ASSIGNED = 1,
            BOARDED = 2,
            ADMITTED_IN_HOSPITAL = 3,
            CLOSED = 4,
        }
        public enum VehicleWiseTicketStatus
        {

            ASSIGNED = 0,
            OPEN = 1,
            REJECTED = 2,
            CLOSED = 3
        }
        public enum PacketType
        {
            LOGIN = 1,
            GENERAL = 2
        }
        public enum DeviceWisePinValue
        {
            COMMONA = 17,
            COMMONB = 941
        }
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
