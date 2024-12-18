using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Configurations;
using RSC_Models;

namespace RSC_DataAccess
{
    public class StoreDBAccess
    {
        public  StoreDBAccess(StoreModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    string StoreProcedure = "PushToStoreDataToDB";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StoreName", model.StoreName);
                        cmd.Parameters.AddWithValue("@StoreCode", model.StoreCode);
                        cmd.Parameters.AddWithValue("@Location", model.Location);
                        cmd.Parameters.AddWithValue("@Manager", model.ManagerName);
                        cmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
