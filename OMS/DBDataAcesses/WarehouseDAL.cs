using FileModel;
using System;
using System.Data.SqlClient;
using System.Data;
using Configuration;
using FileModel;
using ProjectHelpers;
using Enum;
using OMS_IDAL;
namespace OMSDAL
{
    public class WarehouseDAL : BaseProcessor,IWareHouseDAL
    {
        public   void PushWareHouseDataToDB(WareHouseModel wareHouseModel, bool isValidFile, string wareHouseFilePath)
        {
            if (!isValidFile)
            {
                FileHelper.MoiveFile(wareHouseFilePath,FileStatus.Failure);
                return;
            }
            SqlConnection conn = null;

            try
            {
                using ( conn = new SqlConnection(oMSConnectionString))
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
                FileHelper.MoiveFile(wareHouseFilePath, FileStatus.Success);
               
            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(wareHouseFilePath, FileStatus.Failure);

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
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
