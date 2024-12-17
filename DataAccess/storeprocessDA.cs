using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using DataBaseConfig;
using System.Data;
using System.Reflection;


namespace DataAccess
{
    public class storeprocessDA
    {
        public  static List<storemodel> GetAllStoresFromDB()

        {
            var stores = new List<storemodel>();
            using (SqlConnection con = new SqlConnection(ConfigHelper.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT StoreIdPk,StoreCode,StoreName,Location,ManagerName,ContactNumber FROM Stores", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var model = new storemodel();
                            model.StoreId = Convert.ToInt32(reader["StoreIdPk"]);
                            model.StoreCode = Convert.ToString(reader["StoreCode"]);
                            model.StoreName = Convert.ToString(reader["Storename"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            stores.Add(model);
                        }
                    }
                }
                con.Close();
            }
            return stores;
        }
        public static void syncstoreTabledata(storemodel modelObj)

        {
          
            using (SqlConnection con = new SqlConnection(ConfigHelper.connectionString))
            {

                string query = $"Update Stores Set StoreName = @StoreName, Location = @Location, ManagerName= @Manager, ContactNumber = @ContactNumber where StoreCode = @StoreCode";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@storename", DbType.String).Value = modelObj.StoreName;
                    cmd.Parameters.Add("@Location", DbType.String).Value = modelObj.Location;
                    cmd.Parameters.Add("@Manager", DbType.String).Value = modelObj.ManagerName;
                    cmd.Parameters.Add("@ContactNumber", DbType.String).Value = modelObj.ContactNumber;
                    cmd.Parameters.Add("@StoreCode", DbType.String).Value = modelObj.StoreCode;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
