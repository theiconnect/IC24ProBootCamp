using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using FileModel;

namespace OMS.DataAccessLayer_Muni
{
    public class GetWareHousesDataFromDb:DBHelper
    {
        public static List<WareHouseModel> getAllWareHouses()
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

    }
}
