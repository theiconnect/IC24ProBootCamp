using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;



namespace DataAcess
{
        public class StoreDA
        {
            private string isValidFile { get; set; }
            protected static string rscConnectedString { get; set; }

            public static void SyncStoreDataToDB(StoreModel storeModelObject)
            {


                using (SqlConnection con = new SqlConnection(rscConnectedString))
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

