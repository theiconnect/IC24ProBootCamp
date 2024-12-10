﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    internal class InventoryProcess:BaseProcessor
    {
        private string InventoryPath { get; set; }
        private string FailedReason { get; set; }
        private DateTime date {  get; set; }
        private string StockDateStr { get; set; }

        private bool isValidFile { get; set; }
        List<InventoryModel> inventoryList { get; set; }
        List<ProductMasterModel> productMasterList { get; set; }
        List<DBStockData> dBStockDatas { get; set; }

        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(InventoryPath)); }
        }
        public InventoryProcess( string inventorypath) 
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
            using (StreamReader reader = new StreamReader(InventoryPath))
            {
                //int noOfrowsEffected = default(int);
                string jsonFileReader = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(jsonFileReader);
                JArray inventoryArray = (JArray)jsonObject["inventory"];

                inventoryList = inventoryArray.ToObject<List<InventoryModel>>();
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
                foreach(var data in inventoryList)
                {
                    if (data.wareHouseCode != dirName)
                    {
                        FailedReason = "log the error : the record doesn't matches the current warehosue code";

                    }
                    if(!DateTime.TryParse(data.date.ToString(), out DateTime date))
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
                    date=Convert.ToDateTime(data.date);
                    StockDateStr =date.ToString("yyyy-MM-dd");
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

            SyncFileStockWithDB();


        }

        private void SyncProducts()
        {
            string query = string.Empty;

            GetAllProductsFromDB();
            foreach(var stock in inventoryList)
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

        private void GetAllProductsFromDB()
        {
            productMasterList = new List<ProductMasterModel>();
            using (SqlConnection conn=new SqlConnection(oMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($" Select Productidpk,productcode,productname, priceperunit from products", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                          ProductMasterModel model = new ProductMasterModel();
                            model.ProductIdPk=reader.GetInt32(0);
                            model.ProductIdPk = Convert.ToInt32(reader["Productidpk"]);
                            model.ProductCode = reader.GetString(1);
                            model.ProductName = reader.GetString(2);
                            model.PricePerUnit= reader.GetDecimal(3);
                            productMasterList.Add(model);
                        }
                    }
                    conn.Close();


                        

                }


            }
        }

        private void GetAllStockInfoOfTodayFromDB()
        {
            dBStockDatas = new List<DBStockData>();
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"select InventoryIdpk,WarehouseIdfk,ProductIdfk,Date,AvailableQuantity,PricePerUnit,RemainingQuantity from inventory  where date='{StockDateStr}'",conn))
                {
                    conn.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBStockData model = new DBStockData();
                            model.InventoryIdPk = reader.GetInt32(0);
                            model.WareHouseIdFk = reader.GetInt32(1);
                            model.ProductIdFk = reader.GetInt32(2);
                            model.Date=reader.GetDateTime(3);
                            model.QuantityAvailable = reader.GetDecimal(4);
                            model.PricePerUnit = reader.GetDecimal(5);
                            model.RemainingQuantity = reader.GetDecimal(6);
                            dBStockDatas.Add(model);
                        }
                    }
                }
            }


        }

        private void SyncFileStockWithDB()
        {
            foreach (var stock in inventoryList)
            {
                if (!dBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
                {
                    using (SqlConnection con = new SqlConnection(oMSConnectionString))
                    {
                        con.Open();
                        string query = "INSERT INTO inventory( WarehouseIdFk,ProductIdFk, Date, AvailableQuantity, PricePerUnit,RemainingQuantity)" +
                            $"values((select WarehouseIdpk from warehouse where warehousecode='{dirName}'),{stock.ProductIdFk} , '{StockDateStr}', {stock.availableQuantity}, {stock.pricePerUnit},{stock.remainingQuantity})";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
