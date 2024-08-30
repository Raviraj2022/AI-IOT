using System;
using System.Collections.Generic;
using System.Text;

namespace AJ_KITCHEN_SENS_SVC.Entities
{
    public class BaseFrame
    {
    }
    public class LoginInfo
    {
        public string Header { get; set; }
        public string DeviceType { get; set; }
        public string PacketType { get; set; }
        public string ImeiNo { get; set; }
        public string PinNo { get; set; }
        public string Checksum { get; set; }
        public string EndByte { get; set; }
    }
    public class GeneralDataInfo
    {
        public string Header { get; set; }
        public string DeviceType { get; set; }
        public string DataType { get; set; }
        public string PacketType { get; set; }
        public string PacketNumber { get; set; }
        public string ImeiNo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        ///<summary>Start Main Energy Meter-1</summary>

        public string M_E_AVG_PF { get; set; }
        public string M_E_PWR_R { get; set; }
        public string M_E_VOL_R { get; set; }
        public string M_E_CUR_R { get; set; }
        public string M_E_PWR_Y { get; set; }
        public string M_E_VOL_Y { get; set; }
        public string M_E_CUR_Y { get; set; }
        public string M_E_PWR_B { get; set; }
        public string M_E_VOL_B { get; set; }
        public string M_E_CUR_B { get; set; }
        public string M_E_CUMM_ENRGY { get; set; }
        public string M_E_RUN_HR { get; set; }
        public string M_E_TOTAL_PWR { get; set; }

        ///<summary>End Main Energy Meter-1</summary>

        ///<summary>Start DC Energy Meter-1</summary>
        public string DC_E_CUMM_CONS { get; set; }
        public string DC_E_PWR_R { get; set; }
        public string DC_E_PWR_Y { get; set; }
        public string DC_E_PWR_B { get; set; }
        public string DC_E_RUN_HRS { get; set; }
        public string DC_E_TOTAL_HRS { get; set; }
        public string DC_E_TOTAL_PWR { get; set; }
        public string DC_E_PWR_FCTR { get; set; }

        ///<summary>End DC Energy Meter-1</summary>

        ///<summary>Start HVAC 1</summary>

        public string HVAC_CUMM_CONS { get; set; }
        public string HVAC_Z1 { get; set; }
        public string HVAC_Z2 { get; set; }
        public string HVAC_Z3 { get; set; }
        public string HVAC_RUN_HRS { get; set; }
        public string HVAC_TOTAL_PWR { get; set; }
        public string HVAC_PWR_FCTR { get; set; }
        ///<summary>End HVAC 1</summary>


        ///<summary>Start Light Meter</summary>
        public string L_MTR_CUMM_CONS { get; set; }
        public string L_MTR_DIN_PWR { get; set; }
        public string L_MTR_PROD_PWR { get; set; }
        public string L_MTR_SIN_PWR { get; set; }
        public string L_MTR_RUN_HR { get; set; }
        public string L_MTR_TOTAL_PWR { get; set; }
        public string L_MTR_PWR_FCTR { get; set; }

        ///<summary>End Light Meter</summary>

        ///<summary>Start Kitchen 1</summary>
        public string K_1_CUMM_CONS { get; set; }
        public string K_1_R { get; set; }
        public string K_1_Y { get; set; }
        public string K_1_B { get; set; }
        public string K_1_RUN_HR { get; set; }
        public string K_1_TOTAL_PWR { get; set; }
        public string K_1_PWR_FCTR { get; set; }

        ///<summary>End Kitchen 1</summary>

        ///<summary>Start Kitchen 2</summary>
        public string K_2_CUMM_CONS { get; set; }
        public string K_2_R { get; set; }
        public string K_2_Y { get; set; }
        public string K_2_B { get; set; }
        public string K_2_RUN_HR { get; set; }
        public string K_2_TOTAL_PWR { get; set; }
        public string K_2_PWR_FCTR { get; set; }

        ///<summary>End Kitchen 2</summary>


        ///<summary>Start Referigrator Meter</summary>
        public string REF_MTR_CUMM_CONS { get; set; }
        public string REF_MTR_R { get; set; }
        public string REF_MTR_Y { get; set; }
        public string REF_MTR_B { get; set; }
        public string REF_MTR_RUN_HR { get; set; }
        public string REF_MTR_TOTAL_PWR { get; set; }
        public string REF_MTR_PWR_FCTR { get; set; }

        ///<summary>End Kitchen 2</summary>

        public string EndByte { get; set; }
    }
    public class HeartBeatDataInfo
    {
        public string Header { get; set; }
        public string DeviceType { get; set; }
        public string PacketType { get; set; }
        public string PacketNumber { get; set; }
        public string ImeiNo { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string EndByte { get; set; }
    }
}
