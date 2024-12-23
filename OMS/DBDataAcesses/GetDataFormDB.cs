using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Configuration;
using FileModel;
namespace DBDataAcesses
{
    public class GetDataFromDB: BaseProcessor
    {
        public static List<WareHouseModel> GetAllWareHouses()
        {
            var wareHouses = new List<WareHouseModel>();
            SqlConnection i = null;
            try
            {
                using (i = new SqlConnection(oMSConnectionString))
                {

                    using (var cmd = new SqlCommand("GetAllWareHouseData", i))
                    {
                        i.Open();
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
                        if (i.State == ConnectionState.Open)
                        {
                            i.Close();
                        }
                    }
                    return wareHouses;
                }
            }
            catch (Exception ex)
            {
                if (i.State == ConnectionState.Open)
                {
                    i.Close();
                }

            }
            finally
            {
                if (i != null && i.State == ConnectionState.Open)
                {
                    i.Close();
                }
                i.Dispose();
            }
            return wareHouses;


        }
    }
}
