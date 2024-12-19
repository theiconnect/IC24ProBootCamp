using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using FileModel;
using OMS.DataAccessLayer_Muni;
using System.Data;
using OMS.WareHouseDAL_Muni.InventoryDAL;

namespace FileProcessses
{
    public class InventoryProcess:DBHelper
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

        public void Process()
        {
            //Read the file
            //Validate the file 
            //push in to db
            ReadFileData();
            ValidateInventoryData();
            PushInventoryDataToDB();
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
        private void ValidateInventoryData()
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

        private void PushInventoryDataToDB()
        {

            if (!string.IsNullOrEmpty(FailedReason))
            {
                return;
            }
            SyncInventoryFileDataToDB inventory = new SyncInventoryFileDataToDB();

            inventory.GetAllProductsFromDB();

            inventory.SyncProducts(inventoryList, productMasterList);

            inventory.GetAllStockInfoOfTodayFromDB(StockDateStr);

            inventory.SyncFileStockWithDB(inventoryList, dBStockDatas, StockDateStr,dirName);

        }                       
        
    }
}
