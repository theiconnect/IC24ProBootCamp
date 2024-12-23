using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using OMS_Arjun_V3;
//using FileTypes;
//using FileHelper;
using Model;
using ConnectionConfig;
using OMS_IDataAccessLayer;


namespace DataAccessLayer
{
    
    public class WareHouseDAL:ConnectionConfig.ConnectionConfig1,IWareHouseDAL
    {
        public WareHouseModel wareHouseModel { get; set; }
        //private string[] WareHouseFileContent { get; set; }
        public bool PushWareHouseDataToDB(WareHouseModel wareHouseModel)
        {           

            
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

                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("" + ex.Message);
                return false;
            }
            finally
            {
                conn.Dispose();
            }
            
        }
     

        public   List<WareHouseModel> getAllWareHousesFromDB()
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
