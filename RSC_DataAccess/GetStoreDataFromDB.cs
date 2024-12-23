using RSC_Configurations;
using RSC_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_DataAccess
{
    public class GetStoreDataFromDB
    {
        public static List<StoreModel> GetStorecodesFromDB()
        {
            List<StoreModel> storescodes = new List<StoreModel>();
            using (SqlConnection conn = new SqlConnection(AppConfiguration.dbConnectionstring))
            {
                using (SqlCommand cmd = new SqlCommand("select storeIDPk,storeCode,StoreName,Location,ManagerName,ContactNumber from stores", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            StoreModel model = new StoreModel();

                            model.StoreId = Convert.ToInt32(reader["storeIDPk"]);
                            model.StoreCode = Convert.ToString(reader["storeCode"]);
                            model.StoreName = Convert.ToString(reader["StoreName"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            storescodes.Add(model);
                        }
                    }
                }
                conn.Close();
            }
            return storescodes;
        }
    }
}
