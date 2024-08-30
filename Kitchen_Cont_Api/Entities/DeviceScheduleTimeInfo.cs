using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kitchen_Cont_Api.Entities
{
    public class DeviceScheduleSetTimeInfo
    {
        [Required]
        public string ImeiNo { get; set; }
        [Required]
        public string ChannelNo { get; set; }
        [Required]
        public string SchNo { get; set; }
        [Required]
        public string StartTimeHH { get; set; }
        [Required]
        public string StartTimeMM { get; set; }
        [Required]
        public string EndTimeHH { get; set; }
        [Required]
        public string EndTimeMM { get; set; }
    }
    public class DeviceScheduleGetTimeInfo
    {
        [Required]
        public string ImeiNo { get; set; }
        [Required]
        public string ChannelNo { get; set; }
        [Required]
        public string SchNo { get; set; }

    }
    public class OutputStatusInfo
    {
        [Required]
        public string ImeiNo { get; set; }
        [Required]
        public string ChannelNo { get; set; }


    }
    public class DeviceGetDateTimeInfo
    {
        [Required]
        public string ImeiNo { get; set; }


    }
    public class DeviceOutputControlInfo
    {
        [Required]
        public string ImeiNo { get; set; }
        [Required]
        public string ChannelNo { get; set; }
        [Required]
        public string State { get; set; }
    }
    public class DeviceSchedulerTimeExtInfo
    {
        [Required]
        public string ImeiNo { get; set; }
        [Required]
        public string ChannelNo { get; set; }
        [Required]
        public string Alert { get; set; }
        [Required]
        public string TimeHH { get; set; }
        [Required]
        public string TimeMM { get; set; }
        [Required]
        public string TimeSS { get; set; }
        [Required]
        public string DateDD { get; set; }
        [Required]
        public string DateMM { get; set; }
        [Required]
        public string DateYY { get; set; }
    }
    public class SetDateAndTimeDeviceInfo
    {
        [Required]
        public string ImeiNo { get; set; }

        [Required]
        public string TimeHH { get; set; }
        [Required]
        public string TimeMM { get; set; }
        [Required]
        public string TimeSS { get; set; }
        [Required]
        public string DateDD { get; set; }
        [Required]
        public string DateMM { get; set; }
        [Required]
        public string DateYY { get; set; }
    }
}