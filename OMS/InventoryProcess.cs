using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OMS
{
    internal class InventoryProcess : BaseProcessor
    {
        private string InventoryPath { get; set; }
        private string FailedReason { get; set; }
        private DateTime Date { get; set; }
        private string StockDateStr { get; set; }

        private bool IsValidFile { get; set; }
        List<InventoryModel> inventoryList { get; set; }
        List<ProductMasterModel> productMasterList { get; set; }
        List<DBStockData> dBStockDatas { get; set; }

        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(InventoryPath)); }
        }
        public InventoryProcess(string inventorypath)
        {

            InventoryPath = inventorypath;
        }

        internal void Process()
        {
            //Read the file
            //Validate the file 
            //push in to db
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
        }


        private void ReadFileData()
        {
            try
            {
                using (StreamReader reader = new StreamReader(InventoryPath))
                {
                    //int noOfrowsEffected = default(int);
                    string jsonFileReader = reader.ReadToEnd();
                    JObject jsonObject = JObject.Parse(jsonFileReader);
                    JArray inventoryArray = (JArray)jsonObject["inventory"];

                    inventoryList = inventoryArray.ToObject<List<InventoryModel>>();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        private void ValidateStoreData()
        {
            if (inventoryList.Count < 1)
            {
                FailedReason = "Log the error: Invalid file";
            }
            //validate if file has only header row
            else if (inventoryList.Count == 1)
            {
                FailedReason = "Log the warning: No data present in the file";
            }
            else
            {
                foreach (var data in inventoryList)
                {
                    if (data.wareHouseCode != dirName)
                    {
                        FailedReason = "log the error : the record doesn't matches the current warehosue code";

                    }
                    if (!DateTime.TryParse(data.date.ToString(), out DateTime date))
                    {
                        FailedReason += "log the error : the record doesn't have valid date";

                    }
                    if (!decimal.TryParse(data.availableQuantity.ToString(), out decimal result))
                    {
                        FailedReason += $"Error: invalid quantity avaiable data; value:{data.availableQuantity};recordNumber:{data};";
                    }
                    if (!decimal.TryParse(data.pricePerUnit.ToString(), out decimal price))
                    {
                        FailedReason += $"Error: invalid date; value:{data.pricePerUnit};recordNumber:{data};";
                    }
                    if (!decimal.TryParse(data.remainingQuantity.ToString(), out decimal rem))
                    {
                        FailedReason += $"Error: invalid price; value:{data.remainingQuantity};recordNumber:{data} ;";
                    }

                    if (!string.IsNullOrEmpty(FailedReason))
                    {
                        break;
                    }
                    Date = Convert.ToDateTime(data.date);
                    StockDateStr = Date.ToString("yyyy-MM-dd");
                }



            }


        }

        private void PushStoreDataToDB()
        {

            if (!string.IsNullOrEmpty(FailedReason))
            {
                return;
            }

            SyncProducts();

            GetAllStockInfoOfTodayFromDB();

            SyncStockFileWithDB();


        }

        private void SyncProducts()
        {
            string query = string.Empty;

            GetAllProductsFromDB();
            try
            {
                foreach (var stock in inventoryList)
                {

                    var product = productMasterList.Find(x => x.ProductCode == stock.productCode);
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
                    using (SqlConnection con = new SqlConnection(oMSConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    GetAllProductsFromDB();
                    foreach (var stock in inventoryList)
                    {
                        if (Convert.ToInt32(stock.ProductIdFk) == 0)
                        {
                            var product = productMasterList.FirstOrDefault(x => x.ProductCode == stock.productCode);
                            stock.ProductIdFk = product.ProductIdPk;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GetAllProductsFromDB()
        {
            productMasterList = new List<ProductMasterModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
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
                                productMasterList.Add(model);
                            }
                        }
                        conn.Close();

                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        
        private void GetAllStockInfoOfTodayFromDB()
        {
            dBStockDatas = new List<DBStockData>();
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"GetAllStockInfoOfTodayFromDB", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@StockDateStr", DbType.Date).Value = Convert.ToDateTime(StockDateStr);
                        cmd.Parameters.Add(new SqlParameter("@StockDateStr", SqlDbType.DateTime) { Value = Date });

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
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void SyncStockFileWithDB()
        {
            try
            {

               foreach (var stock in inventoryList)
               { 
            
                    if (!dBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
                    {
                        using (SqlConnection con = new SqlConnection(oMSConnectionString))
                        {
                            con.Open();
                      
                            using (SqlCommand cmd = new SqlCommand("AddInventory", con))
                            {
                                cmd.CommandType=CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@WarehouseCode", dirName);
                                cmd.Parameters.AddWithValue("@ProductIdFk", stock.ProductIdFk);
                                cmd.Parameters.AddWithValue("@StockDate", StockDateStr);
                                cmd.Parameters.AddWithValue("@AvailableQuantity", stock.availableQuantity);
                                cmd.Parameters.AddWithValue("@PricePerUnit", stock.pricePerUnit);
                                cmd.Parameters.AddWithValue("@RemainingQuantity", stock.remainingQuantity);

                                cmd.ExecuteNonQuery();
                            }
                            con.Close();    
                        }
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
