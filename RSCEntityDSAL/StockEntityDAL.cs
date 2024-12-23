using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_IDAL;

namespace RSCEntityDAL
{
    internal class StockEntityDAL : AppConnection, IStockDAL
    {

        public void SyncStockTableData(List<Stockmodel> stocks)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(rSCConnectionString))
                {
                    string StoreProcedure = "StockDataToDB";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        foreach (var stockData in stocks)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@productcode", DbType.Int32).Value = stockData.ProductCode;
                            cmd.Parameters.Add("@storeid", DbType.Int32).Value = stockData.Storeidfk;
                            cmd.Parameters.Add("@date", DbType.Int32).Value = stockData.date;
                            cmd.Parameters.Add("@QuantityAvailable", DbType.Int32).Value = stockData.QuantityAvailable;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}





