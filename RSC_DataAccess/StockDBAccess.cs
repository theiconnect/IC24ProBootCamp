using RSC_Configurations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;
using RSC_IDAL;

namespace RSC_DataAccess
{
    public class StockDBAccess:IStockDAL
    {
        public  bool StockDBAcces(List<Stockmodel> stocks , int Storeid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    string StoreProcedure = "StockDataToDB";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        foreach (var stockData in stocks)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@productcode", DbType.Int32).Value = stockData.productCode;
                            cmd.Parameters.Add("@storeid", DbType.Int32).Value = stockData.storeidfk;
                            cmd.Parameters.Add("@date", DbType.Int32).Value = stockData.date;
                            cmd.Parameters.Add("@QuantityAvailable", DbType.Int32).Value = stockData.QuantityAvailable;
                            int Rowsaffected = cmd.ExecuteNonQuery();
                        }

                    }
                    con.Close();

                }
            }
             
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
        private List<ProductModel> GetAllProducts()
        {
            using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
            {
                List<ProductModel> products = new List<ProductModel>();
                string StoreProcedure = "GetAllProducts";
                using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                {
                    con.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            ProductModel model = new ProductModel();
                            model.ProductIdPk = Convert.ToInt32(reader["ProductidPk"]);
                            model.ProductName = Convert.ToString(reader["ProductName"]);
                            model.ProductCode = Convert.ToString(reader["productcode"]);
                            model.PricePerUnit = Convert.ToDecimal(reader["priceperunit"]);
                            products.Add(model);
                        }
                        con.Close();
                    }
                }
                return products;
            }
        }
    }
}
