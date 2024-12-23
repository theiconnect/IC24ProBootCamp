using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using Configuration;
using OMS.IDataAccessLayer_Muni;

namespace OMS.DataAccessLayer_Muni
{
    public class InventoryDAL:DBHelper,IInventoryDAL
    {
        
        public void SyncProducts(InventoryModel stock)
        {
                    SqlConnection conn = null;
                    try
                    {
                        
                        using (conn = new SqlConnection(oMSConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("USP_SyncProducts", conn))
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.Add("@productCode", DbType.String).Value = stock.productCode;
                                cmd.Parameters.Add("@productName", DbType.String).Value = stock.productCode;
                                cmd.Parameters.Add("@pricePerUnit", DbType.Decimal).Value = stock.pricePerUnit;
                                cmd.Parameters.Add("@categoryIdfk", DbType.Int32).Value = 1;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                    finally
                    {
                        if (conn != null && conn.State == ConnectionState.Open) { conn.Dispose(); }
                    }
        }
        public List<ProductMasterModel> GetAllProductsFromDB( )
        {
            SqlConnection conn = null;
            List<ProductMasterModel> productMasterList = new List<ProductMasterModel>();
            try
            {

                using (conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllProductsFromDB", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                                productMasterList.Add(model);
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open) { conn.Dispose(); }
            }
            return productMasterList;
        }
        public List<DBStockData> GetAllStockInfoOfTodayFromDB(string StockDateStr)
        {
            List<DBStockData> dBStockDatas = new List<DBStockData>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllStockInfoOfTodayFromDB", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StockDateStr", DbType.Date).Value = StockDateStr;
                        conn.Open();
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
                                dBStockDatas.Add(model);
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                
                throw;

            }
            finally 
            {
                if (conn != null && conn.State == ConnectionState.Open) 
                {
                    conn.Dispose(); 
                } 

            }
            return dBStockDatas;

        }
        public void SyncFileStockWithDB(InventoryModel stock, string StockDateStr,string dirName)
        {
            SqlConnection conn = null;
                    try
                    {
                        using (conn = new SqlConnection(oMSConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("AddInventory", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@WarehouseCode", DbType.String).Value = dirName;
                                cmd.Parameters.Add("@ProductIdFk", DbType.Int32).Value = stock.ProductIdFk;
                                cmd.Parameters.Add("@StockDate", DbType.DateTime).Value = StockDateStr;
                                cmd.Parameters.Add("@AvailableQuantity", DbType.Int32).Value = stock.availableQuantity;
                                cmd.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = stock.pricePerUnit;
                                cmd.Parameters.Add("@RemainingQuantity", DbType.Int32).Value = stock.remainingQuantity;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
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
                        conn.Dispose();
                    }
            
        }

    }
}
