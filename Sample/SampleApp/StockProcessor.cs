using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SampleApp
{
    internal class StockProcessor : BaseProcessor
    {
        private bool isValidFile { get; set; }
        private DateTime StockDate { get; set; }
        private string StockDateStr { get { return StockDate.ToString("yyyy-MM-dd"); } }
        private string FailReason { get; set; }
        private string FilePath { get; set; }
        private string DirName { get { return Path.GetDirectoryName(FilePath); } }
        private string[] FileContent { get; set; }
        private List<StockBO> FileStock { get; set; }
        private List<StockBO> DBStock { get; set; }
        private List<ProductMasterModel> Products { get; set; }
        private int StoreId {  get; set; }

        public StockProcessor(string filePath, int storeId)
        {
            FilePath = filePath;
            StoreId = storeId;
        }

        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
        }

        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(FilePath);
        }

        private void ValidateStoreData()
        {
            //validate if file has no content
            if (FileContent.Length < 1)
            {
                FailReason = "Log the error: Invalid file";
            }
            //validate if file has only header row
            else if (FileContent.Length == 1)
            {
                FailReason = "Log the warning: No data present in the file";
            }
            else
            {
                for(int i = 1; i < FileContent.Length; i++)
                {
                    string[] data = FileContent[i].Split(';');
                    if (data.Length != 6)
                    {
                        FailReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.";
                        break;
                    }
                    
                    if (data[1].ToLower() != DirName.ToLower())
                    {
                        FailReason = "Log the error: Invalid data; store code not matching with current storecode.;";
                    }

                    if (!decimal.TryParse(data[3], out decimal result))
                    {
                        FailReason += $"Error: invalid quantity avaiable data; value:{data[3]};recordNumber:{i};";
                    }
                    if (!DateTime.TryParse(data[4], out DateTime dt))
                    {
                        FailReason += $"Error: invalid date; value:{data[4]};recordNumber:{i};";
                    }
                    if(!decimal.TryParse(data[5], out decimal price))
                    {
                        FailReason += $"Error: invalid price; value:{data[5]};recordNumber:{i} ;";
                    }
                    if (!string.IsNullOrEmpty(FailReason))
                        break;
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                isValidFile = false;
                //log this error message into a file
                return;
            }
            isValidFile = true;
        }

        private void PushStoreDataToDB()
        {
            if(!isValidFile)
            {
                return;
            }

            PrepareStockObjects();

            SyncProducts();

            GetAllStockInfoOfTodayFromDB();

            SyncFileStockWithDB();
        }

        private void SyncFileStockWithDB()
        {
            foreach (var stock in FileStock)
            {
                if(!DBStock.Exists(s=> s.Date == stock.Date && s.ProductIdFk == stock.ProductIdFk))
                {
                    using (SqlConnection con = new SqlConnection(rSCConnectionString))
                    {
                        con.Open();
                        string query = "INSERT INTO Stock(ProductIdFk, StoreIdFk, Date, QuantityAvailable, PricePerUnit)" +
                            $"values({stock.ProductIdFk}, {stock.StoreIdFk}, {stock.Date}, {stock.QuantityAvailable}, {stock.PricePerUnit})";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void GetAllStockInfoOfTodayFromDB()
        {
            DBStock = new List<StockBO>();
            using (SqlConnection con = new SqlConnection())
            {
                string query = $"SELECT StockIdPk, ProductIdFk, StoreIdFk, Date, QuantityAvailable, PricePerUnit From Stock where Date= {this.StockDateStr}";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stock = new StockBO();

                            stock.StockIdPk = Convert.ToInt32(reader["StockIdPk"]);
                            stock.ProductIdFk = Convert.ToInt32(reader["ProductIdFk"]);
                            stock.StoreIdFk = Convert.ToInt32(reader["StoreIdFk"]);
                            stock.Date = Convert.ToDateTime(reader["Date"]);
                            stock.QuantityAvailable = Convert.ToDecimal(reader["QuantityAvailable"]);
                            stock.PricePerUnit = Convert.ToDecimal(reader["PricePerUnit"]);

                            DBStock.Add(stock);
                        }
                    }
                }
            }
        }

        private void SyncProducts()
        {
            GetAllProductsFromDB();

            //Insert Product master details if not exists
            string query = string.Empty;
            foreach (var stock in FileStock)
            {
                var product = Products.FirstOrDefault(x => x.ProductCode == stock.ProductCode);
                if (product == null)
                {
                    query = query + "insert into ProductMaster(ProductCode, ProductName, PricePerUnit)" +
                        $"   Values('{stock.ProductCode}', '{stock.ProductName}', {stock.PricePerUnit})" +
                        $"";
                }
                else
                {
                    stock.ProductIdFk = product.ProductIdPk;
                }
            }
            if (!string.IsNullOrEmpty(query))
            {
                using (SqlConnection con = new SqlConnection(rSCConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                GetAllProductsFromDB();
                foreach (var stock in FileStock)
                {
                    if(stock.ProductIdFk == 0)
                    {
                        var product = Products.FirstOrDefault(x => x.ProductCode == stock.ProductCode);
                        stock.ProductIdFk = product.ProductIdPk;
                    }
                }
            }
        }

        private void GetAllProductsFromDB()
        {
            Products = new List<ProductMasterModel>();
            using (SqlConnection con = new SqlConnection())
            {
                string query = "SELECT ProductIdpk, ProductCode, ProductName From ProductMaster";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new ProductMasterModel();
                            product.ProductIdPk = Convert.ToInt32(reader["ProductIdpk"]);//reader.GetInt32(0);
                            product.ProductCode = reader.GetString(1);
                            product.ProductName = reader.GetString(2);
                            Products.Add(product);
                        }
                    }
                }
            }


        }

        private void PrepareStockObjects()
        {
            FileStock = new List<StockBO>();
            for (int i = 1; i < FileContent.Length; i++)
            {
                string record = FileContent[i];
                string[] data = record.Split(';');
                StockBO model = new StockBO();
                model.ProductCode = data[0];
                model.StoreIdFk = this.StoreId;
                model.ProductName = data[2];
                if (decimal.TryParse(data[3], out decimal result))
                    model.QuantityAvailable = result;
                if (DateTime.TryParse(data[4], out DateTime dt))
                {
                    model.Date = dt;
                    StockDate = dt;
                }
                if (decimal.TryParse(data[5], out decimal price))
                    model.PricePerUnit = price;
                FileStock.Add(model);
            }
        }
    }
}
