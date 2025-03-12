using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathAndDataBaseConfig;
using IDataAccess;

namespace DataAccess
{
    public class StockDA:IStockDA
    {
        public void SyncStockData(List<ProductMasterBO> products, List<StockBO> stockFileInformation,  List<ProductMasterBO> StockFileInformation)
        {
            GetrAllProductsFromDB(products);
            
            SyncProductMasterTableData(StockFileInformation);

        }
        
        
        
        public void GetrAllProductsFromDB(List<ProductMasterBO> products)
        {
            products = new List<ProductMasterBO>();
            using (SqlConnection con = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                //SELECT ProductIdPk,ProductCode,ProductName,PricePerUnit FROM ProductMaster
                using (SqlCommand command = new SqlCommand("GetAllProducts", con))
                {
                    try
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
                                products.Add(productModel);


                            }

                        }

                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine("Error:" + ex.Message);
                        throw;

                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();

                        }

                    }
                    
                    
                  
                }
            }
        }

        public bool SyncStockTableData(List<StockBO> stockFileInformation)
        {
            using (SqlConnection connection = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                try
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
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
            }
            return true;
        }
        public void SyncProductMasterTableData(List<ProductMasterBO> StockFileInformation)
        {


            using (SqlConnection connection = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                try
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
                        foreach (var stock in StockFileInformation)
                        {
                            command.Parameters.Clear();
                            command.Parameters.Add("@productCode", DbType.String).Value = stock.ProductCode;
                            command.Parameters.Add("@ProductName", DbType.String).Value = stock.ProductName;
                            command.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = stock.PricePerUnit;
                            command.ExecuteNonQuery();


                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                

            }
        }
    }
}
