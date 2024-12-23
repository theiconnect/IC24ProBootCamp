using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using FileModel;
using OMS.IDataAccessLayer_Muni;

namespace OMS.DataAccessLayer_Muni
{
    public class WareHouseDAL:DBHelper,IWareHouseDAL
    {
        public  List<WareHouseModel> GetAllWareHouses()
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
        public void PushWareHouseDataToDB(WareHouseModel wareHouseModel)
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
