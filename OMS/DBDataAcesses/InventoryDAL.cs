using Enum;
using FileModel;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Configuration;
using OMS_IDAL;
using FileModel.models;

namespace DBDataAcesses
{
    public class InventoryDAL : BaseProcessor, IInventoryDAL
    {

        public bool PushInvetoryDataToDB(InventoryPushData data)
        {



            SyncProducts(data);

            data.DBStockDatas = GetAllStockInfoOfTodayFromDB(data);

            return SyncStockFileWithDB(data);


        }
        public void SyncProducts(InventoryPushData data)
        {
            string query = string.Empty;

            data.ProductMasterList = GetAllProductsFromDB(data);
            SqlConnection conn = null;
            try
            {
                foreach (var stock in data.InventoryList)
                {

                    var product = data.ProductMasterList.Find(x => x.ProductCode == stock.productCode);
                    if (product == null)
                    {
                        query = query + "insert into Products(ProductCode, ProductName, PricePerUnit,categoryidfk)" +
                            $"   Values('{stock.productCode}', '{stock.productCode}', {stock.pricePerUnit},{1})" +
                            $"";
                    }
                    else
                    {
                        stock.ProductIdFk = product.ProductIdPk;
                    }
                }
                if (!string.IsNullOrEmpty(query))
                {
                    using (conn = new SqlConnection(oMSConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                    data.ProductMasterList = GetAllProductsFromDB(data);
                    foreach (var stock in data.InventoryList)
                    {
                        if (Convert.ToInt32(stock.ProductIdFk) == 0)
                        {
                            var product = data.ProductMasterList.FirstOrDefault(x => x.ProductCode == stock.productCode);
                            stock.ProductIdFk = product.ProductIdPk;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn != null)
                    conn.Dispose();
            }
        }
        public List<ProductMasterModel> GetAllProductsFromDB(InventoryPushData data)
        {
            data.ProductMasterList = new List<ProductMasterModel>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllProductsFromDB", conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ProductMasterModel model = new ProductMasterModel();
                                model.ProductIdPk = reader.GetInt32(0);
                                model.ProductIdPk = Convert.ToInt32(reader["Productidpk"]);
                                model.ProductCode = reader.GetString(1);
                                model.ProductName = reader.GetString(2);
                                model.PricePerUnit = reader.GetDecimal(3);
                                data.ProductMasterList.Add(model);
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }


                }
                return data.ProductMasterList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return null;
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

        public List<DBStockData> GetAllStockInfoOfTodayFromDB(InventoryPushData data)
        {
            data.DBStockDatas = new List<DBStockData>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"GetAllStockInfoOfTodayFromDB", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@StockDateStr", SqlDbType.DateTime) { Value = data.Date });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DBStockData model = new DBStockData();
                                model.InventoryIdPk = reader.GetInt32(0);
                                model.WareHouseIdFk = reader.GetInt32(1);
                                model.ProductIdFk = reader.GetInt32(2);
                                model.Date = reader.GetDateTime(3);
                                model.QuantityAvailable = reader.GetDecimal(4);
                                model.PricePerUnit = reader.GetDecimal(5);
                                model.RemainingQuantity = reader.GetDecimal(6);
                                data.DBStockDatas.Add(model);
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                return data.DBStockDatas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return null;
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

        public bool SyncStockFileWithDB(InventoryPushData data)
        {
            SqlConnection conn = null;
            try
            {
                var count = 0;
                foreach (var stock in data.InventoryList)
                {

                    if (!data.DBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
                    {
                        using (conn = new SqlConnection(oMSConnectionString))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand("AddInventory", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@WarehouseCode", data.DirName);
                                cmd.Parameters.AddWithValue("@ProductIdFk", stock.ProductIdFk);
                                cmd.Parameters.AddWithValue("@StockDate", data.StockDateStr);
                                cmd.Parameters.AddWithValue("@AvailableQuantity", stock.availableQuantity);
                                cmd.Parameters.AddWithValue("@PricePerUnit", stock.pricePerUnit);
                                cmd.Parameters.AddWithValue("@RemainingQuantity", stock.remainingQuantity);
                                cmd.ExecuteNonQuery();

                                count++;
                            }
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }
                    }
                }

                FileHelper.LogEntries($"[{DateTime.Now}] INFO: The Inventory file which is  associated with the warehouse code {data.DirName} is suceesfully processed and the file is moved into processed folder.Rows affeceted:{count} \n");
                return true;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);

                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {data.DirName} is invalid file and got the exception {ex.Message}.Please check and update the file. \n");
                return false;
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn != null)
                    conn.Dispose();
            }
        }


    }
    
     

}

