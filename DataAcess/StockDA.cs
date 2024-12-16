using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class StockDA
    {
        private List<ProductMasterBO> Products { get; set; }
        private List<StockBO> stockFileInformation { get; set; }
        protected static string rscConnectedString { get; set; }
        public void SyncStockData()
        {
            GetrAllProductsFromDB();
            SyncStockTableData();
            SyncProductMasterTableData();

        }
        private void GetrAllProductsFromDB()
        {
            Products = new List<ProductMasterBO>();
            using (SqlConnection con = new SqlConnection(rscConnectedString))
            {
                //SELECT ProductIdPk,ProductCode,ProductName,PricePerUnit FROM ProductMaster
                using (SqlCommand command = new SqlCommand("GetAllProducts", con))
                {
                    con.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductMasterBO productModel = new ProductMasterBO();
                            productModel.ProductIdPk = Convert.ToInt32(reader["ProductIdPk"]);
                            productModel.ProductCode = Convert.ToString(reader["ProductCode"]);
                            productModel.ProductName = Convert.ToString(reader["ProductName"]);
                            productModel.PricePerUnit = Convert.ToDecimal(reader["PricePerUnit"]);
                            Products.Add(productModel);


                        }

                    }
                    con.Close();
                }
            }
        }

        private void SyncStockTableData()
        {
            using (SqlConnection connection = new SqlConnection(rscConnectedString))
            {

                using (SqlCommand command = new SqlCommand())
                {//INSERT INTO stock (ProductIdFk, StoreIdFk, QuantityAvailable, Date) " +
                 // "VALUES ((SELECT ProductIdPk FROM ProductMaster WHERE productCode = @ProductCode), " +
                 // "(SELECT StoreIdPk FROM stores WHERE storeCode = @StoreCode),@QuantityAvailable, @Date)
                    command.CommandText = "Insert_Stocks";

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    foreach (var stock in stockFileInformation)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add("@QuantityAvailable", DbType.Decimal).Value = stock.QuantityAvailable;
                        command.Parameters.Add("@ProductCode", DbType.String).Value = stock.ProductCode;
                        command.Parameters.Add("@StoreCode", DbType.String).Value = stock.StoreCode;
                        command.Parameters.Add("@Date", DbType.DateTime).Value = stock.Date;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();


                }
            }


        }
        private void SyncProductMasterTableData()
        {


            using (SqlConnection connection = new SqlConnection(rscConnectedString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    //If Not Exists(select ProductCode from ProductMaster where ProductCode=@ProductCode)\r\nbegin\n
                    //INSERTINTO ProductMaster (ProductIdPk,ProductCode,ProductName,PricePerUnit)
                    //VALUES((select max(ProductIdPk)+1 from productmaster),@ProductCode, @ProductName, @PricePerUnit)\r\nend"
                    command.CommandText = "InsertProducts";
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    foreach (var stock in stockFileInformation)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add("@productCode", DbType.String).Value = stock.ProductCode;
                        command.Parameters.Add("@ProductName", DbType.String).Value = stock.ProductName;
                        command.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = stock.PricePerUnit;
                        command.ExecuteNonQuery();


                    }

                    connection.Close();


                }

            }
        }
    }
}
