using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kitchen_Cont_Api.Helper
{
    public class Constants
    {
        public const string FAILURE = "SOMETHING WRONG HAPPEND";
        public const string SENT_SUCCESS = "DATA SENT SUCCESSFULLY";
        public const string GET_SUCCESS = "DATA GET SUCCESSFULLY";
        public const string DEVICE_NOT_CONNECTED = "DEVICE NOT CONNECTED";
        public const string INPUT_REQUIRED = "INPUT FIELDS ARE REQUIRED";
        public const string SEPARATOR = ",";
        public const string HEADER = "@$";
        //public static readonly string END_BYTE = "$" + Environment.NewLine;
        public const string END_BYTE = "#";
        public const string IMEI_APPEND = "_A";

    }
}