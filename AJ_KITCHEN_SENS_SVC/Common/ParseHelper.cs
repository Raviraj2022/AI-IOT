using AJ_KITCHEN_SENS_SVC.Entities;
using NHAI_AIS140_Server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AJ_KITCHEN_SENS_SVC.Common
{
    public class ParseHelper
    {
        public static bool IsLogin(string sb)
        {

            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == ((int)Enums.PacketType.LOGIN).ToString())
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsGeneralData_Old(string HexVal)
        {
            bool Result = false;
            try
            {
                // List<string> _lstdata = sb.ToString().Split(',').ToList();
                string packetdata = HexVal.Replace("2C", ",");
                List<string> _lstdata = packetdata.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    //if (sb.ToString(5, 1) == "2")
                    // if (sb.ToString(0, 1) == "2")
                    string PacketType = CommonHelper.HexToDecimal(_lstdata[2]);
                    if (PacketType == "2")
                        Result = true;
                    //return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsGeneralData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    //if (sb.ToString(5, 1) == "2")
                    // if (sb.ToString(0, 1) == "2")
                    string PacketType = _lstdata[2];
                    if (PacketType == "2")
                        Result = true;
                    //return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsHearBeatData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == "3")
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsSetScheduleTimeData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Set_Time))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsGetScheduleTimeData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Get_Time))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsOutputStatusData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Output_Status))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsDateTimeDeviceData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Get_Date_Time_Device))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsOutputControlData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Ouput_Control_Packet))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsScheduleTimeExtData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Scheduler_Time_Extension))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static bool IsSetTimeDateData(string sb)
        {
            bool Result = false;
            try
            {
                List<string> _lstdata = sb.Split(',').ToList();
                if (_lstdata.Count() > 0)
                {
                    string PacketType = _lstdata[2];
                    if (PacketType == Convert.ToString((int)Enums.PacketTypeInfo.Set_Time_Date))
                        Result = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Result;
        }
        public static LoginInfo CastListToLoginInfo(string sb)
        {
            LoginInfo info = new LoginInfo();
            List<string> _lstdata = sb.Split(',').ToList();
            try
            {
                if (_lstdata.Count() > 0)
                {
                    info.Header = _lstdata[0];
                    info.DeviceType = _lstdata[1];//CommonHelper.ToHexString(_lstdata[1]);
                    info.PacketType = _lstdata[2];// CommonHelper.ToHexString(_lstdata[2]);
                    info.ImeiNo = _lstdata[3];
                    info.PinNo = _lstdata[4];
                    info.Checksum = _lstdata[5];
                    info.EndByte = _lstdata[6];

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;

        }
        public static GeneralDataInfo CastListToGeneralInfo(string sb, string HexVal)
        {
            GeneralDataInfo info = new GeneralDataInfo();
            List<string> _lstdata = sb.Split(',').ToList();
            //string packetdata = HexVal.Replace("2C", ",");
            //List<string> _packetlstdata = packetdata.Split(',').ToList();
            List<string> _packetlstdata = _lstdata;
            try
            {
                if (_lstdata.Count() > 0)
                {
                    info.Header = _packetlstdata[0];
                    info.DeviceType = _packetlstdata[1];
                    info.PacketType = _packetlstdata[2];
                    info.DataType = _packetlstdata[3];
                    info.PacketNumber = _packetlstdata[4];
                    info.Time = _packetlstdata[5];
                    info.Date = _packetlstdata[6];

                    info.M_E_AVG_PF = _packetlstdata[7];
                    info.M_E_PWR_R = _packetlstdata[8];
                    info.M_E_VOL_R = _packetlstdata[9];
                    info.M_E_CUR_R = _packetlstdata[10];
                    info.M_E_PWR_Y = _packetlstdata[11];
                    info.M_E_VOL_Y = _packetlstdata[12];
                    info.M_E_CUR_Y = _packetlstdata[13];
                    info.M_E_PWR_B = _packetlstdata[14];
                    info.M_E_VOL_B = _packetlstdata[15];
                    info.M_E_CUR_B = _packetlstdata[16];
                    info.M_E_CUMM_ENRGY = _packetlstdata[17];
                    info.M_E_RUN_HR = _packetlstdata[18];
                    info.M_E_TOTAL_PWR = _packetlstdata[19];

                    info.DC_E_CUMM_CONS = _packetlstdata[20];
                    info.DC_E_PWR_R = _packetlstdata[21];
                    info.DC_E_PWR_Y = _packetlstdata[22];
                    info.DC_E_PWR_B = _packetlstdata[23];
                    info.DC_E_RUN_HRS = _packetlstdata[24];
                    info.DC_E_TOTAL_HRS = _packetlstdata[25];
                    info.DC_E_TOTAL_PWR = _packetlstdata[26];
                    info.DC_E_PWR_FCTR = _packetlstdata[27];

                    info.HVAC_CUMM_CONS = _packetlstdata[28];
                    info.HVAC_Z1 = _packetlstdata[29];
                    info.HVAC_Z2 = _packetlstdata[30];
                    info.HVAC_Z3 = _packetlstdata[31];
                    info.HVAC_RUN_HRS = _packetlstdata[32];
                    info.HVAC_TOTAL_PWR = _packetlstdata[33];
                    info.HVAC_PWR_FCTR = _packetlstdata[34];

                    info.L_MTR_CUMM_CONS = _packetlstdata[35];
                    info.L_MTR_DIN_PWR = _packetlstdata[36];
                    info.L_MTR_PROD_PWR = _packetlstdata[37];
                    info.L_MTR_SIN_PWR = _packetlstdata[38];
                    info.L_MTR_RUN_HR = _packetlstdata[39];
                    info.L_MTR_TOTAL_PWR = _packetlstdata[40];
                    info.L_MTR_PWR_FCTR = _packetlstdata[41];

                    info.K_1_CUMM_CONS = _packetlstdata[42];
                    info.K_1_R = _packetlstdata[43];
                    info.K_1_Y = _packetlstdata[44];
                    info.K_1_B = _packetlstdata[45];
                    info.K_1_RUN_HR = _packetlstdata[46];
                    info.K_1_TOTAL_PWR = _packetlstdata[47];
                    info.K_1_PWR_FCTR = _packetlstdata[48];

                    info.K_2_CUMM_CONS = _packetlstdata[49];
                    info.K_2_R = _packetlstdata[50];
                    info.K_2_Y = _packetlstdata[51];
                    info.K_2_B = _packetlstdata[52];
                    info.K_2_RUN_HR = _packetlstdata[53];
                    info.K_2_TOTAL_PWR = _packetlstdata[54];
                    info.K_2_PWR_FCTR = _packetlstdata[55];

                    info.REF_MTR_CUMM_CONS = _packetlstdata[56];
                    info.REF_MTR_R = _packetlstdata[57];
                    info.REF_MTR_Y = _packetlstdata[58];
                    info.REF_MTR_B = _packetlstdata[59];
                    info.REF_MTR_RUN_HR = _packetlstdata[60];
                    info.REF_MTR_TOTAL_PWR = _packetlstdata[61];
                    info.REF_MTR_PWR_FCTR = _packetlstdata[62];

                    info.EndByte = "#";//_packetlstdata[63];//CommonHelper.ConvertHex(_packetlstdata[63]);


                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;

        }
        public static GeneralDataInfo CastListToGeneralInfo_V1(string sb, string HexVal)
        {
            GeneralDataInfo info = new GeneralDataInfo();
            List<string> _lstdata = sb.Split(',').ToList();
            string packetdata = HexVal.Replace("2C", ",");
            List<string> _packetlstdata = packetdata.Split(',').ToList();
            try
            {
                if (_lstdata.Count() > 0)
                {
                    info.Header = CommonHelper.ConvertHex(_packetlstdata[0].Substring(0, 2)) + CommonHelper.ConvertHex(_packetlstdata[0].Substring(2, 2));
                    info.DeviceType = CommonHelper.HexToDecimal(_packetlstdata[1]);
                    info.PacketType = CommonHelper.HexToDecimal(_packetlstdata[2]);
                    info.DataType = CommonHelper.HexToDecimal(_packetlstdata[3]);
                    info.PacketNumber = CommonHelper.HexToDecimal(_packetlstdata[4]);
                    info.Time = CommonHelper.HexToDecimal(_packetlstdata[5].Substring(0, 2)) + CommonHelper.HexToDecimal(_packetlstdata[5].Substring(2, 2)) + CommonHelper.HexToDecimal(_packetlstdata[5].Substring(4, 2)); ;
                    info.Date = CommonHelper.HexToDecimal(_packetlstdata[6].Substring(0, 2)) + CommonHelper.HexToDecimal(_packetlstdata[6].Substring(2, 2)) + CommonHelper.HexToDecimal(_packetlstdata[6].Substring(4, 2));

                    info.M_E_AVG_PF = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[7].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[7].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[7].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[7].Substring(6, 2)));
                    info.M_E_PWR_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[8].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[8].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[8].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[8].Substring(6, 2)));
                    info.M_E_VOL_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[9].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[9].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[9].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[9].Substring(6, 2)));
                    info.M_E_CUR_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[10].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[10].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[10].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[10].Substring(6, 2)));
                    info.M_E_PWR_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[11].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[11].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[11].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[11].Substring(6, 2)));
                    info.M_E_VOL_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[12].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[12].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[12].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[12].Substring(6, 2)));
                    info.M_E_CUR_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[13].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[13].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[13].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[13].Substring(6, 2)));
                    info.M_E_PWR_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[14].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[14].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[14].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[14].Substring(6, 2)));
                    info.M_E_VOL_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[15].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[15].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[15].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[15].Substring(6, 2)));
                    info.M_E_CUR_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[16].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[16].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[16].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[16].Substring(6, 2)));
                    info.M_E_CUMM_ENRGY = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[17].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[17].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[17].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[17].Substring(6, 2)));
                    info.M_E_RUN_HR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[18].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[18].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[18].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[18].Substring(6, 2)));
                    info.M_E_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[19].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[19].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[19].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[19].Substring(6, 2)));

                    info.DC_E_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[20].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[20].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[20].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[20].Substring(6, 2)));
                    info.DC_E_PWR_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[21].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[21].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[21].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[21].Substring(6, 2)));
                    info.DC_E_PWR_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[22].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[22].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[22].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[22].Substring(6, 2)));
                    info.DC_E_PWR_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[23].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[23].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[23].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[23].Substring(6, 2)));
                    info.DC_E_RUN_HRS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[24].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[24].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[24].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[24].Substring(6, 2)));
                    info.DC_E_TOTAL_HRS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[25].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[25].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[25].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[25].Substring(6, 2)));
                    info.DC_E_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[26].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[26].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[26].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[26].Substring(6, 2)));
                    info.DC_E_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[27].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[27].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[27].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[27].Substring(6, 2)));

                    info.HVAC_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[28].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[28].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[28].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[28].Substring(6, 2)));
                    info.HVAC_Z1 = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[29].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[29].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[29].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[29].Substring(6, 2)));
                    info.HVAC_Z2 = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[30].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[30].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[30].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[30].Substring(6, 2)));
                    info.HVAC_Z3 = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[31].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[31].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[31].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[31].Substring(6, 2)));
                    info.HVAC_RUN_HRS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[32].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[32].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[32].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[32].Substring(6, 2)));
                    info.HVAC_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[33].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[33].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[33].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[33].Substring(6, 2)));
                    info.HVAC_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[34].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[34].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[34].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[34].Substring(6, 2)));

                    info.L_MTR_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[35].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[35].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[35].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[35].Substring(6, 2)));
                    info.L_MTR_DIN_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[36].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[36].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[36].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[36].Substring(6, 2)));
                    info.L_MTR_PROD_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[37].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[37].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[37].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[37].Substring(6, 2)));
                    info.L_MTR_SIN_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[38].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[38].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[38].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[38].Substring(6, 2)));
                    info.L_MTR_RUN_HR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[39].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[39].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[39].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[39].Substring(6, 2)));
                    info.L_MTR_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[40].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[40].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[40].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[40].Substring(6, 2)));
                    info.L_MTR_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[41].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[41].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[41].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[41].Substring(6, 2)));

                    info.K_1_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[42].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[42].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[42].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[42].Substring(6, 2)));
                    info.K_1_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[43].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[43].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[43].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[43].Substring(6, 2)));
                    info.K_1_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[44].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[44].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[44].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[44].Substring(6, 2)));
                    info.K_1_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[45].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[45].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[45].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[45].Substring(6, 2)));
                    info.K_1_RUN_HR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[46].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[46].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[46].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[46].Substring(6, 2)));
                    info.K_1_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[47].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[47].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[47].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[47].Substring(6, 2)));
                    info.K_1_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[48].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[48].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[48].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[48].Substring(6, 2)));

                    info.K_2_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[49].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[49].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[49].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[49].Substring(6, 2)));
                    info.K_2_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[50].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[50].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[50].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[50].Substring(6, 2)));
                    info.K_2_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[51].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[51].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[51].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[51].Substring(6, 2)));
                    info.K_2_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[52].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[52].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[52].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[52].Substring(6, 2)));
                    info.K_2_RUN_HR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[53].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[53].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[53].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[53].Substring(6, 2)));
                    info.K_2_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[54].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[54].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[54].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[54].Substring(6, 2)));
                    info.K_2_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[55].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[55].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[55].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[55].Substring(6, 2)));

                    info.REF_MTR_CUMM_CONS = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[56].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[56].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[56].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[56].Substring(6, 2)));
                    info.REF_MTR_R = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[57].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[57].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[57].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[57].Substring(6, 2)));
                    info.REF_MTR_Y = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[58].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[58].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[58].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[58].Substring(6, 2)));
                    info.REF_MTR_B = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[59].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[59].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[59].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[59].Substring(6, 2)));
                    info.REF_MTR_RUN_HR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[60].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[60].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[60].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[60].Substring(6, 2)));
                    info.REF_MTR_TOTAL_PWR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[61].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[61].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[61].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[61].Substring(6, 2)));
                    info.REF_MTR_PWR_FCTR = CommonHelper.FloatValueCalc4Bytes(CommonHelper.HexToDecimal(_packetlstdata[62].Substring(0, 2)), CommonHelper.HexToDecimal(_packetlstdata[62].Substring(2, 2)), CommonHelper.HexToDecimal(_packetlstdata[62].Substring(4, 2)), CommonHelper.HexToDecimal(_packetlstdata[62].Substring(6, 2)));

                    info.EndByte = CommonHelper.ConvertHex(_packetlstdata[63]);


                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;

        }
        public static GeneralDataInfo CastListToGeneralInfo_Old(string sb, string HexVal)
        {
            GeneralDataInfo info = new GeneralDataInfo();
            List<string> _lstdata = sb.Split(',').ToList();
            string packetdata = HexVal.Replace("2C", ",");
            List<string> _packetlstdata = packetdata.Split(',').ToList();
            try
            {
                if (_lstdata.Count() > 0)
                {
                    info.Header = _lstdata[0];
                    info.DeviceType = CommonHelper.ToHexString(_lstdata[1]);
                    info.PacketType = CommonHelper.ToHexString(_lstdata[2]);
                    info.DataType = CommonHelper.ToHexString(_lstdata[3]);
                    info.PacketNumber = CommonHelper.ToHexString(_lstdata[4]);
                    info.Time = CommonHelper.ToHexString(_lstdata[5]);
                    info.Date = CommonHelper.ToHexString(_lstdata[6]);

                    info.M_E_AVG_PF = CommonHelper.ToHexString(_lstdata[7]);
                    info.M_E_PWR_R = CommonHelper.ToHexString(_lstdata[8]);
                    info.M_E_VOL_R = CommonHelper.ToHexString(_lstdata[9]);
                    info.M_E_CUR_R = CommonHelper.ToHexString(_lstdata[10]);
                    info.M_E_PWR_Y = CommonHelper.ToHexString(_lstdata[11]);
                    info.M_E_VOL_Y = CommonHelper.ToHexString(_lstdata[12]);
                    info.M_E_CUR_Y = CommonHelper.ToHexString(_lstdata[13]);
                    info.M_E_PWR_B = CommonHelper.ToHexString(_lstdata[14]);
                    info.M_E_VOL_B = CommonHelper.ToHexString(_lstdata[15]);
                    info.M_E_CUR_B = CommonHelper.ToHexString(_lstdata[16]);
                    info.M_E_CUMM_ENRGY = CommonHelper.ToHexString(_lstdata[17]);
                    info.M_E_RUN_HR = CommonHelper.ToHexString(_lstdata[18]);
                    info.M_E_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[19]);

                    info.DC_E_CUMM_CONS = CommonHelper.ToHexString(_lstdata[20]);
                    info.DC_E_PWR_R = CommonHelper.ToHexString(_lstdata[21]);
                    info.DC_E_PWR_Y = CommonHelper.ToHexString(_lstdata[22]);
                    info.DC_E_PWR_B = CommonHelper.ToHexString(_lstdata[23]);
                    info.DC_E_RUN_HRS = CommonHelper.ToHexString(_lstdata[24]);
                    info.DC_E_TOTAL_HRS = CommonHelper.ToHexString(_lstdata[25]);
                    info.DC_E_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[26]);
                    info.DC_E_PWR_FCTR = CommonHelper.ToHexString(_lstdata[27]);

                    info.HVAC_CUMM_CONS = CommonHelper.ToHexString(_lstdata[28]);
                    info.HVAC_Z1 = CommonHelper.ToHexString(_lstdata[29]);
                    info.HVAC_Z2 = CommonHelper.ToHexString(_lstdata[30]);
                    info.HVAC_Z3 = CommonHelper.ToHexString(_lstdata[31]);
                    info.HVAC_RUN_HRS = CommonHelper.ToHexString(_lstdata[32]);
                    info.HVAC_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[33]);
                    info.HVAC_PWR_FCTR = CommonHelper.ToHexString(_lstdata[34]);

                    info.L_MTR_CUMM_CONS = CommonHelper.ToHexString(_lstdata[35]);
                    info.L_MTR_DIN_PWR = CommonHelper.ToHexString(_lstdata[36]);
                    info.L_MTR_PROD_PWR = CommonHelper.ToHexString(_lstdata[37]);
                    info.L_MTR_SIN_PWR = CommonHelper.ToHexString(_lstdata[38]);
                    info.L_MTR_RUN_HR = CommonHelper.ToHexString(_lstdata[39]);
                    info.L_MTR_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[40]);
                    info.L_MTR_PWR_FCTR = CommonHelper.ToHexString(_lstdata[41]);

                    info.K_1_CUMM_CONS = CommonHelper.ToHexString(_lstdata[42]);
                    info.K_1_R = CommonHelper.ToHexString(_lstdata[43]);
                    info.K_1_Y = CommonHelper.ToHexString(_lstdata[44]);
                    info.K_1_B = CommonHelper.ToHexString(_lstdata[45]);
                    info.K_1_RUN_HR = CommonHelper.ToHexString(_lstdata[46]);
                    info.K_1_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[47]);
                    info.K_1_PWR_FCTR = CommonHelper.ToHexString(_lstdata[48]);

                    info.K_2_CUMM_CONS = CommonHelper.ToHexString(_lstdata[49]);
                    info.K_2_R = CommonHelper.ToHexString(_lstdata[50]);
                    info.K_2_Y = CommonHelper.ToHexString(_lstdata[51]);
                    info.K_2_B = CommonHelper.ToHexString(_lstdata[52]);
                    info.K_2_RUN_HR = CommonHelper.ToHexString(_lstdata[53]);
                    info.K_2_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[54]);
                    info.K_2_PWR_FCTR = CommonHelper.ToHexString(_lstdata[55]);

                    info.REF_MTR_CUMM_CONS = CommonHelper.ToHexString(_lstdata[56]);
                    info.REF_MTR_R = CommonHelper.ToHexString(_lstdata[57]);
                    info.REF_MTR_Y = CommonHelper.ToHexString(_lstdata[58]);
                    info.REF_MTR_B = CommonHelper.ToHexString(_lstdata[59]);
                    info.REF_MTR_RUN_HR = CommonHelper.ToHexString(_lstdata[60]);
                    info.REF_MTR_TOTAL_PWR = CommonHelper.ToHexString(_lstdata[61]);
                    info.REF_MTR_PWR_FCTR = CommonHelper.ToHexString(_lstdata[62]);

                    info.EndByte = _lstdata[63];


                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;

        }
        public static HeartBeatDataInfo CastListToHeartBeatInfo(string sb)
        {
            HeartBeatDataInfo info = new HeartBeatDataInfo();
            List<string> _lstdata = sb.Split(',').ToList();
            try
            {
                if (_lstdata.Count() > 0)
                {
                    info.Header = _lstdata[0];

                    //info.DeviceType = _lstdata[1];
                    info.PacketType = _lstdata[1];
                    // info.DataType = _lstdata[3];
                    info.PacketNumber = _lstdata[2];
                    info.Data1 = _lstdata[3];
                    info.Data2 = _lstdata[4];

                    info.EndByte = _lstdata[5];


                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;

        }

        public static string PinNoCalculation(LoginInfo info)
        {
            string Result = string.Empty;
            int PinAValue = 0;
            int PinBValue = 0;
            long SumOfDigit = CommonHelper.GetSumOfDigit(info.ImeiNo);
            switch (info.DeviceType)
            {
                case "101":
                    PinAValue = (int)Enums.DeviceWisePinValue.COMMONA;
                    PinBValue = (int)Enums.DeviceWisePinValue.COMMONB;
                    break;
                default:
                    Result = string.Empty;
                    break;
            }
            Result = (PinAValue * (SumOfDigit) + PinBValue).ToString();
            return Result;
        }
    }
}
