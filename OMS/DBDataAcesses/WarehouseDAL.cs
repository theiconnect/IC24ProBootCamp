using FileModel;
using System;
using System.Data.SqlClient;
using System.Data;
using Configuration;
using FileModel;
using ProjectHelpers;
using Enum;
using OMS_IDAL;
using System.Collections.Generic;
namespace OMSDAL
{
    public class WarehouseDAL : BaseProcessor, IWareHouseDAL
    {

        public List<WareHouseModel> GetAllWareHouses()
        {
            var wareHouses = new List<WareHouseModel>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {

                    using (var cmd = new SqlCommand("GetAllWareHouseData", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var wareHouse = new WareHouseModel();
                                wareHouse.WareHouseidpk = Convert.ToInt32(reader["WarehouseIdPk"]);
                                wareHouse.WareHouseCode = Convert.ToString(reader["WarehouseCode"]);
                                wareHouse.WareHouseName = Convert.ToString(reader["WarehouseName"]);
                                wareHouse.Location = Convert.ToString(reader["Location"]);
                                wareHouse.ManagerName = Convert.ToString(reader["ManagerName"]);
                                wareHouse.ContactNumber = Convert.ToString(reader["ContactNo"]);
                                wareHouses.Add(wareHouse);
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return wareHouses;
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
            return wareHouses;


        }

        public bool PushWareHouseDataToDB(WareHouseModel wareHouseModel)
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
                return true;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
                return false;
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
