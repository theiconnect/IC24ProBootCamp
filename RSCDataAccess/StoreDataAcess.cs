using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSCModels;
using RSCConfigiration;
using RSCIDataAcess;

namespace RSCDataAccess
{
    public class StoreDataAcess: StoreIDataAcess
    {
        public static List<StoreModel> GetAllStoresFromDB()
        {
            List<StoreModel> models = new List<StoreModel>();
            using (SqlConnection connection = new SqlConnection(AppConnection.rSCConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select StoreIdPk, StoreCode, StoreName, Location, ManagerName, ContactNumber FROM Stores", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoreModel storeModelObj = new StoreModel();
                            storeModelObj.StoreId = Convert.ToInt32(reader["StoreIdPk"]);
                            storeModelObj.StoreCode = Convert.ToString(reader["StoreCode"]);
                            storeModelObj.StoreName = Convert.ToString(reader["StoreName"]);
                            storeModelObj.Location = Convert.ToString(reader["Location"]);
                            storeModelObj.ManagerName = Convert.ToString(reader["ManagerName"]);
                            storeModelObj.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            models.Add(storeModelObj);
                        }
                    }

                }
                connection.Close();

            }
            return models;
        }

        public void PushStoreData(StoreModel storeData)
        {

            
            using (SqlConnection con = new SqlConnection(AppConnection.rSCConnectionString))
            {
                string query = "Update Stores set StoreName=@StoreName,Location=@Location,ManagerName=@ManagerName,ContactNumber=@ContactNumber where StoreCode=@StoreCode";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@StoreName", DbType.String).Value = storeData.StoreName;
                    cmd.Parameters.Add("@Location", DbType.String).Value = storeData.Location;
                    cmd.Parameters.Add("@ManagerName", DbType.String).Value = storeData.ManagerName;
                    cmd.Parameters.Add("@ContactNumber", DbType.String).Value = storeData.ContactNumber;
                    cmd.Parameters.Add("@StoreCode", DbType.String).Value = storeData.StoreCode;
                    cmd.ExecuteNonQuery();
                }

            }
        }



    }
}
