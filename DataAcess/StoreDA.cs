using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using PathAndDataBaseConfig;



namespace DataAcess
{

     public class StoreDA

     {
        private static List<StoreModel> GetAllStoresDataFromDB()
        {
            List<StoreModel> stores = new List<StoreModel>();
            using (SqlConnection con = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                //string query = "select StoreIdPk,storeCode,StoreName,Location,ManagerName,StoreContactNumber from stores";
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandText = "GetAllStores";
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoreModel model = new StoreModel();
                            model.StoreIdPk = Convert.ToInt32(reader["StoreIdPk"]);
                            model.StoreCode = Convert.ToString(reader["storeCode"]);
                            model.StoreName = Convert.ToString(reader["StoreName"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["StoreContactNumber"]);
                            stores.Add(model);
                        }

                    }
                }
                con.Close();
            }
            return stores;
        }


        public static void SyncStoreDataToDB(StoreModel storeModelObject)
        {


                using (SqlConnection con = new SqlConnection(BaseProcessor.rscConnectedString))
                {
                    //string query = "update stores set storeName=@StoreName,Location=@Location,ManagerName=@ManagerName,StoreContactNumber=@ContactNumber where StoreCode=@StoreCode";
                    //string query1 = $"update stores set storeName={storeModelObject.StoreName},Location=@Location,ManagerName=@ManagerName,StoreContactNumber=@ContactNumber where StoreCode=@StoreCode";

                    using (SqlCommand command = new SqlCommand())
                    {
                        con.Open();
                        command.CommandText = "update_Stores";
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.Add("@StoreName", DbType.String).Value = storeModelObject.StoreName;
                            command.Parameters.Add("@StoreCode", DbType.String).Value = storeModelObject.StoreCode;
                            command.Parameters.Add("@Location", DbType.String).Value = storeModelObject.Location;
                            command.Parameters.Add("@ManagerName", DbType.String).Value = storeModelObject.ManagerName;
                            command.Parameters.Add("@ContactNumber", DbType.String).Value = storeModelObject.ContactNumber;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);

                        }
                        command.ExecuteNonQuery();
                        con.Close();

                    }

                }


        }


     }
}

