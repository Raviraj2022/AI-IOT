using AJ_KITCHEN_SENS_SVC.Common;
using AJ_KITCHEN_SENS_SVC.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace AJ_KITCHEN_SENS_SVC
{
    public class OperationContext
    {

        internal ResponseInfo PushPacketData(string info, DateTime IDate)
        {
            ResponseInfo result = new ResponseInfo();
            //DateTime IDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            try
            {
                using (SqlConnection con = new SqlConnection(Program.settings.SQL_ConnectionStr))
                {
                    using (SqlCommand cmd = new SqlCommand("spAddKitchenInfo"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@InstTxt", info);
                        cmd.Parameters.AddWithValue("@TDate", IDate);


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            result = dt.AsEnumerable().Select(sdr => new ResponseInfo()
                            {

                                Result = sdr["Result"].ToString(),
                                Msg = sdr["Msg"].ToString(),

                            }).FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.Msg = "Something wrong Happend";
                result.Result = "0";
            }
            return result;
        }
        internal ResponseInfo PushGeneralPacketData(GeneralDataInfo info, DateTime IDate)
        {
            ResponseInfo result = new ResponseInfo();
            //DateTime IDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            try
            {
                using (SqlConnection con = new SqlConnection(Program.settings.SQL_ConnectionStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SpAddKGeneralPacketInfo"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DeviceType", info.DeviceType);
                        cmd.Parameters.AddWithValue("@DataType", info.DataType);
                        cmd.Parameters.AddWithValue("@PacketType", info.PacketType);
                        cmd.Parameters.AddWithValue("@PacketNumber", info.PacketNumber);
                        cmd.Parameters.AddWithValue("@ImeiNo", info.ImeiNo);
                        cmd.Parameters.AddWithValue("@DeviceDate", info.Date);
                        cmd.Parameters.AddWithValue("@DeviceTime", info.Time);
                        cmd.Parameters.AddWithValue("@M_E_AVG_PF", info.M_E_AVG_PF);
                        cmd.Parameters.AddWithValue("@M_E_PWR_R", info.M_E_PWR_R);
                        cmd.Parameters.AddWithValue("@M_E_VOL_R", info.M_E_VOL_R);
                        cmd.Parameters.AddWithValue("@M_E_CUR_R", info.M_E_CUR_R);
                        cmd.Parameters.AddWithValue("@M_E_PWR_Y", info.M_E_PWR_Y);
                        cmd.Parameters.AddWithValue("@M_E_VOL_Y", info.M_E_VOL_Y);
                        cmd.Parameters.AddWithValue("@M_E_CUR_Y", info.M_E_CUR_Y);
                        cmd.Parameters.AddWithValue("@M_E_PWR_B", info.M_E_PWR_B);
                        cmd.Parameters.AddWithValue("@M_E_VOL_B", info.M_E_VOL_B);
                        cmd.Parameters.AddWithValue("@M_E_CUR_B", info.M_E_CUR_B);
                        cmd.Parameters.AddWithValue("@M_E_CUMM_ENRGY", info.M_E_CUMM_ENRGY);
                        cmd.Parameters.AddWithValue("@M_E_RUN_HR", info.M_E_RUN_HR);
                        cmd.Parameters.AddWithValue("@M_E_TOTAL_PWR", info.M_E_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@DC_E_CUMM_CONS", info.DC_E_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@DC_E_PWR_R", info.DC_E_PWR_R);
                        cmd.Parameters.AddWithValue("@DC_E_PWR_Y", info.DC_E_PWR_Y);
                        cmd.Parameters.AddWithValue("@DC_E_PWR_B", info.DC_E_PWR_B);
                        cmd.Parameters.AddWithValue("@DC_E_RUN_HRS", info.DC_E_RUN_HRS);
                        cmd.Parameters.AddWithValue("@DC_E_TOTAL_HRS", info.DC_E_TOTAL_HRS);
                        cmd.Parameters.AddWithValue("@DC_E_TOTAL_PWR", info.DC_E_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@DC_E_PWR_FCTR", info.DC_E_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@HVAC_CUMM_CONS", info.HVAC_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@HVAC_Z1", info.HVAC_Z1);
                        cmd.Parameters.AddWithValue("@HVAC_Z2", info.HVAC_Z2);
                        cmd.Parameters.AddWithValue("@HVAC_Z3", info.HVAC_Z3);
                        cmd.Parameters.AddWithValue("@HVAC_RUN_HRS", info.HVAC_RUN_HRS);
                        cmd.Parameters.AddWithValue("@HVAC_TOTAL_PWR", info.HVAC_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@HVAC_PWR_FCTR", info.HVAC_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@L_MTR_CUMM_CONS", info.L_MTR_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@L_MTR_DIN_PWR", info.L_MTR_DIN_PWR);
                        cmd.Parameters.AddWithValue("@L_MTR_PROD_PWR", info.L_MTR_PROD_PWR);
                        cmd.Parameters.AddWithValue("@L_MTR_SIN_PWR", info.L_MTR_SIN_PWR);
                        cmd.Parameters.AddWithValue("@L_MTR_RUN_HR", info.L_MTR_RUN_HR);
                        cmd.Parameters.AddWithValue("@L_MTR_TOTAL_PWR", info.L_MTR_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@L_MTR_PWR_FCTR", info.L_MTR_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@K_1_CUMM_CONS", info.K_1_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@K_1_R", info.K_1_R);
                        cmd.Parameters.AddWithValue("@K_1_Y", info.K_1_Y);
                        cmd.Parameters.AddWithValue("@K_1_B", info.K_1_B);
                        cmd.Parameters.AddWithValue("@K_1_RUN_HR", info.K_1_RUN_HR);
                        cmd.Parameters.AddWithValue("@K_1_TOTAL_PWR", info.K_1_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@K_1_PWR_FCTR", info.K_1_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@K_2_CUMM_CONS", info.K_2_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@K_2_R", info.K_2_R);
                        cmd.Parameters.AddWithValue("@K_2_Y", info.K_2_Y);
                        cmd.Parameters.AddWithValue("@K_2_B", info.K_2_B);
                        cmd.Parameters.AddWithValue("@K_2_RUN_HR", info.K_2_RUN_HR);
                        cmd.Parameters.AddWithValue("@K_2_TOTAL_PWR", info.K_2_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@K_2_PWR_FCTR", info.K_2_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@REF_MTR_CUMM_CONS", info.REF_MTR_CUMM_CONS);
                        cmd.Parameters.AddWithValue("@REF_MTR_R", info.REF_MTR_R);
                        cmd.Parameters.AddWithValue("@REF_MTR_Y", info.REF_MTR_Y);
                        cmd.Parameters.AddWithValue("@REF_MTR_B", info.REF_MTR_B);
                        cmd.Parameters.AddWithValue("@REF_MTR_RUN_HR", info.REF_MTR_RUN_HR);
                        cmd.Parameters.AddWithValue("@REF_MTR_TOTAL_PWR", info.REF_MTR_TOTAL_PWR);
                        cmd.Parameters.AddWithValue("@REF_MTR_PWR_FCTR", info.REF_MTR_PWR_FCTR);
                        cmd.Parameters.AddWithValue("@SyncOn", IDate);


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            result = dt.AsEnumerable().Select(sdr => new ResponseInfo()
                            {

                                Result = sdr["Result"].ToString(),
                                Msg = sdr["Msg"].ToString(),

                            }).FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.Msg = "Something wrong Happend";
                result.Result = "0";
            }
            return result;
        }
    }


}
