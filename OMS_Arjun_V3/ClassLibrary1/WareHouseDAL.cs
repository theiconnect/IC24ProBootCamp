using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using OMS_Arjun_V3;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;


namespace DataAccessLayer
{
    
    public class WareHouseDAL:ConnectionConfig.ConnectionConfig1
    {
        public WareHouseModel wareHouseModel { get; set; }
        //private string[] WareHouseFileContent { get; set; }
        public void UpdateWareHouseDataToDB(string[] WareHouseFileContent, string WareHouseFilePath)
        {           

            prepareWareHouseObject(WareHouseFileContent);
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"Update warehouse Set warehouseName = '{wareHouseModel.WareHouseName}', Location = '{wareHouseModel.Location}', ManagerName= '{wareHouseModel.ManagerName}', ContactNo = '{wareHouseModel.ContactNo}' where WareHouseCode = '{wareHouseModel.WareHouseCode}'", conn))
                    {

                        cmd.ExecuteNonQuery();
                        
                    }
                    conn.Close();
                }
                FileHelper.FileHelper.MoveFile(WareHouseFilePath, FileTypes.FileTypes1.Success);
            }
            catch (Exception ex) 
            {
                FileHelper.FileHelper.MoveFile(WareHouseFilePath, FileTypes.FileTypes1.Failure);
                Console.WriteLine("" + ex.Message);
            }
            finally
            {
                conn.Dispose();
            }
            
        }
        public void prepareWareHouseObject(string[] wareHouseFileContent)
        {
            wareHouseModel = new WareHouseModel();

            string[] data = wareHouseFileContent[1].Split('|');
            wareHouseModel.WareHouseCode = data[0];
            wareHouseModel.WareHouseName = data[1];
            wareHouseModel.Location = data[2];
            wareHouseModel.ManagerName = data[3];
            wareHouseModel.ContactNo = data[4];
        }

        public static List<WareHouseModel> getAllWareHousesFromDB()
        {
            var wareHouses = new List<WareHouseModel>();
            SqlConnection Con = null;
            try
            {
                using (Con = new SqlConnection(oMSConnectionString))
                {

                    using (var cmd = new SqlCommand("SELECT WarehouseIdPk,WarehouseCode,WarehouseName,Location,ManagerName,ContactNo FROM Warehouse ", Con))
                    {
                        Con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var wareHouse = new WareHouseModel();
                                wareHouse.WareHouseIdpk = Convert.ToInt32(reader["WarehouseIdPk"]);
                                wareHouse.WareHouseCode = Convert.ToString(reader["WarehouseCode"]);
                                wareHouse.WareHouseName = Convert.ToString(reader["WarehouseName"]);
                                wareHouse.Location = Convert.ToString(reader["Location"]);
                                wareHouse.ManagerName = Convert.ToString(reader["ManagerName"]);
                                wareHouse.ContactNo = Convert.ToString(reader["ContactNo"]);
                                wareHouses.Add(wareHouse);
                            }
                        }
                        if (Con.State == ConnectionState.Open)
                        {
                            Con.Close();
                        }
                    }
                    return wareHouses;
                }
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
            finally
            {
                if (Con != null && Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                Con.Dispose();
            }
        }

    }
}
