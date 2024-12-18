using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;

namespace OMS.DataAccessLayer_Muni
{
    public class SyncWareHouseDataToDB:DBHelper
    {
        public static void PushWareHouseDataToDB(WareHouseModel wareHouseModel)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"UpdateWarehousesData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WareHouseCode", wareHouseModel.WareHouseCode);
                        cmd.Parameters.AddWithValue("@WareHouseName", wareHouseModel.WareHouseName);
                        cmd.Parameters.AddWithValue("@Location", wareHouseModel.Location);
                        cmd.Parameters.AddWithValue("@ManagerName", wareHouseModel.ManagerName);
                        cmd.Parameters.AddWithValue("@ContactNO", wareHouseModel.ContactNumber);
                        cmd.ExecuteNonQuery();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

    } 
}
