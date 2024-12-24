//using Enum;
//using FileModel;
//using ProjectHelpers;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using Configuration;
//using OMS_IDAL;

//namespace DBDataAcesses
//{
//    public class InventoryDAL:BaseProcessor
//        public   bool  PushInvetoryDataToDB(string failedReason, List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList, string dirName, string stockDateStr, DateTime date, IInventoryDAL IInventoryDAL)
//        {

           

//            SyncProducts(inventoryList, dBStockDatas, productMasterList);

//            dBStockDatas = GetAllStockInfoOfTodayFromDB(dBStockDatas, date);

//            return SyncStockFileWithDB(inventoryList, dBStockDatas, dirName, stockDateStr);

            
//        }
//       public   void SyncProducts(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList)
//        {
//            string query = string.Empty;

//            productMasterList = GetAllProductsFromDB(productMasterList);
//            SqlConnection conn= null;
//            try
//            {
//                foreach (var stock in inventoryList)
//                {

//                    var product = productMasterList.Find(x => x.ProductCode == stock.productCode);
//                    if (product == null)
//                    {
//                        query = query + "insert into Products(ProductCode, ProductName, PricePerUnit,categoryidfk)" +
//                            $"   Values('{stock.productCode}', '{stock.productCode}', {stock.pricePerUnit},{1})" +
//                            $"";
//                    }
//                    else
//                    {
//                        stock.ProductIdFk = product.ProductIdPk;
//                    }
//                }
//                if (!string.IsNullOrEmpty(query))
//                {
//                    using ( conn = new SqlConnection(oMSConnectionString))
//                    {
//                        conn.Open();
//                        using (SqlCommand cmd = new SqlCommand(query, conn))
//                        {
//                            cmd.ExecuteNonQuery();
//                        }
//                        if (conn.State == ConnectionState.Open)
//                        {
//                            conn.Close();
//                        }
//                    }

//                    productMasterList = GetAllProductsFromDB(productMasterList);
//                    foreach (var stock in inventoryList)
//                    {
//                        if (Convert.ToInt32(stock.ProductIdFk) == 0)
//                        {
//                            var product = productMasterList.FirstOrDefault(x => x.ProductCode == stock.productCode);
//                            stock.ProductIdFk = product.ProductIdPk;
//                        }
//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                if (conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }

//            }
//            finally
//            {
//                if (conn != null && conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                if(conn!=null)
//                conn.Dispose();
//            }
//        }
//       public   List<ProductMasterModel> GetAllProductsFromDB(List<ProductMasterModel> productMasterList)
//        {
//            productMasterList = new List<ProductMasterModel>();
//            SqlConnection conn=null;
//            try
//            {
//                using ( conn = new SqlConnection(oMSConnectionString))
//                {
//                    using (SqlCommand cmd = new SqlCommand("GetAllProductsFromDB", conn))
//                    {
//                        conn.Open();
//                        using (SqlDataReader reader = cmd.ExecuteReader())
//                        {

//                            while (reader.Read())
//                            {
//                                ProductMasterModel model = new ProductMasterModel();
//                                model.ProductIdPk = reader.GetInt32(0);
//                                model.ProductIdPk = Convert.ToInt32(reader["Productidpk"]);
//                                model.ProductCode = reader.GetString(1);
//                                model.ProductName = reader.GetString(2);
//                                model.PricePerUnit = reader.GetDecimal(3);
//                                productMasterList.Add(model);
//                            }
//                        }
//                        if (conn.State == ConnectionState.Open)
//                        {
//                            conn.Close();
//                        }

//                    }


//                }
//                return productMasterList;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                if (conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                return null;
//            }
//            finally
//            {
//                if (conn != null && conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                conn.Dispose();
//            }
//        }
     
//       public   List<DBStockData> GetAllStockInfoOfTodayFromDB(List<DBStockData> dBStockDatas, DateTime date)
//        {
//            dBStockDatas = new List<DBStockData>();
//            SqlConnection conn=null;
//            try
//            {
//                using ( conn = new SqlConnection(oMSConnectionString))
//                {
//                    using (SqlCommand cmd = new SqlCommand($"GetAllStockInfoOfTodayFromDB", conn))
//                    {
//                        conn.Open();
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        //cmd.Parameters.Add("@StockDateStr", DbType.Date).Value = Convert.ToDateTime(StockDateStr);
//                        cmd.Parameters.Add(new SqlParameter("@StockDateStr", SqlDbType.DateTime) { Value = date });

//                        using (SqlDataReader reader = cmd.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                DBStockData model = new DBStockData();
//                                model.InventoryIdPk = reader.GetInt32(0);
//                                model.WareHouseIdFk = reader.GetInt32(1);
//                                model.ProductIdFk = reader.GetInt32(2);
//                                model.Date = reader.GetDateTime(3);
//                                model.QuantityAvailable = reader.GetDecimal(4);
//                                model.PricePerUnit = reader.GetDecimal(5);
//                                model.RemainingQuantity = reader.GetDecimal(6);
//                                dBStockDatas.Add(model);
//                            }
//                        }
//                        if (conn.State == ConnectionState.Open)
//                        {
//                            conn.Close();
//                        }
//                    }
//                }
//                return dBStockDatas;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                if (conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                return null;
//            }
//            finally
//            {
//                if (conn != null && conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                conn.Dispose();
//            }

//        }
       
//       public   bool SyncStockFileWithDB(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, string dirName, string stockDateStr)
//        {
//            SqlConnection conn = null;
//            try
//            {

//                foreach (var stock in inventoryList)
//                {

//                    if (!dBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
//                    {
//                        using ( conn = new SqlConnection(oMSConnectionString))
//                        {
//                            conn.Open();

//                            using (SqlCommand cmd = new SqlCommand("AddInventory", conn))
//                            {
//                                cmd.CommandType = CommandType.StoredProcedure;
//                                cmd.Parameters.AddWithValue("@WarehouseCode", dirName);
//                                cmd.Parameters.AddWithValue("@ProductIdFk", stock.ProductIdFk);
//                                cmd.Parameters.AddWithValue("@StockDate", stockDateStr);
//                                cmd.Parameters.AddWithValue("@AvailableQuantity", stock.availableQuantity);
//                                cmd.Parameters.AddWithValue("@PricePerUnit", stock.pricePerUnit);
//                                cmd.Parameters.AddWithValue("@RemainingQuantity", stock.remainingQuantity);
//                                cmd.ExecuteNonQuery();
//                            }
//                            if (conn.State == ConnectionState.Open)
//                            {
//                                conn.Close();
//                            }
//                        }
//                    }
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                if (conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                Console.WriteLine(ex.Message);
//                return false;
//            }

//            finally
//            {
//                if (conn != null && conn.State == ConnectionState.Open)
//                {
//                    conn.Close();
//                }
//                if(conn!= null)
//                conn.Dispose();
//            }
//        }

//    }
//}
