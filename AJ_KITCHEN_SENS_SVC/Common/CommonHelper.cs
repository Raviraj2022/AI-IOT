using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AJ_KITCHEN_SENS_SVC.Common
{
    public static class CommonHelper
    {
        public static DateTime IndianStandard(DateTime currentDate)
        {
            TimeZoneInfo mountain = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime utc = currentDate;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, mountain);
            //return DateTime.Now;
        }
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static long GetSumOfDigit(string Input)
        {
            long sum = 0, m, n;
            //n = int.Parse(Console.ReadLine());
            n = long.Parse(Input);
            while (n > 0)
            {
                m = n % 10;
                sum = sum + m;
                n = n / 10;
            }
            return sum;
        }

        public static string oldToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public static string ToHexString(string str)
        {
            int Result = 0;
            try
            {
                Result = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception ex)
            {
                return "";
            }

            return Result.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public static string HexToDecimal(string str)
        {
            long n = Int64.Parse(str, System.Globalization.NumberStyles.HexNumber);
            return n.ToString();
        }
        public static string BytesToHex(this byte[] barray, bool toLowerCase = true)
        {
            byte addByte = 0x37;
            if (toLowerCase) addByte = 0x57;
            char[] c = new char[barray.Length * 2];
            byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + addByte : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + addByte : b + 0x30);
            }

            return new string(c);
        }
        public static string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }
        public static bool AllStringPropertyValuesAreNonEmpty(object myObject)
        {
            var allStringPropertyValues =
                from property in myObject.GetType().GetProperties()
                where property.PropertyType == typeof(string) && property.CanRead
                select (string)property.GetValue(myObject);

            return allStringPropertyValues.All(value => !string.IsNullOrEmpty(value));
        }
        public static string FloatValueCalc4Bytes(string Param1, string Param2, string Param3, string Param4)
        {
            int result = (16777216 * Convert.ToInt32(Param1)) + (65536 * Convert.ToInt32(Param2)) + (256 * Convert.ToInt32(Param3)) + Convert.ToInt32(Param4);//154 + (153 << 8) + (25 << 16) + (64 << 24);
            float Fresult = BitConverter.ToSingle(BitConverter.GetBytes(result), 0);
            return Fresult.ToString();
        }
        public const int Device_Recieve_Timeout_Seconds = 5000;
        public const int Device_Clear_Seconds = 7;
        public const int Min_Channel_No = 1;
        public const int Max_Channel_No = 8;
    }
}
