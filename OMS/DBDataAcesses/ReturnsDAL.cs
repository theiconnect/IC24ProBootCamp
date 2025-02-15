using Configuration;
using Enum;
using FileModel;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_IDAL;

namespace DBDataAcesses
{
    public class ReturnsDAL:BaseProcessor,IReturnsDAL
    {
        public   bool PushReturnsDataToDB(List<ReturnsModel> returnsList, string returnFilePath)
        {
            SqlConnection conn=null;
            try
            {
                var count = 0;
                using ( conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "AddReturn";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        foreach (var returnRecord in returnsList)
                        {
                            if (!returnRecord.IsvalidReturn) continue;
                            cmd.Parameters.Clear();

                            // Add parameters with correct DbType and values
                            cmd.Parameters.Add(new SqlParameter("@ReturnDate", SqlDbType.DateTime) { Value = returnRecord.Date });
                            cmd.Parameters.Add(new SqlParameter("@InvoiceNumber", SqlDbType.NVarChar, 50) { Value = returnRecord.InvoiceNumber });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 255) { Value = returnRecord.Reason });
                            cmd.Parameters.Add(new SqlParameter("@ReturnStatus", SqlDbType.NVarChar, 50) { Value = returnRecord.ReturnStatus });
                            cmd.Parameters.Add(new SqlParameter("@AmountRefund", SqlDbType.Decimal) { Value = returnRecord.AmountRefund });
                            
                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();
                            count++;
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }

                FileHelper.LogEntries($"[{DateTime.Now}] INFO: The Returns file is sucessfully processed and moved to Processed folder.Rows affected:{count}\n");
                return true;
            }
            catch (Exception ex)
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Returns file is Invalid File and moved to Error folder.\n");

                return false;
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }

        }

    }
}
