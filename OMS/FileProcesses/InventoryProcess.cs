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
using FileModel;
using FileProcesses;
using Configuration;
using ProjectHelpers;
using DBDataAcesses;
using OMS_IDAL;
using OMSEntityDAL;

namespace FileProcesses
{
    public class InventoryProcess : BaseProcessor
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

        public void Process()
        {
            //Read the file
            //Validate the file 
            //push in to db
            ReadFileData();
            ValidateStoreData();

            //IInventoryDAL inventoryDAL = new InventoryDAL();
            IInventoryDAL inventoryDAL= new InventoryEntityDAL();
            inventoryDAL.PushInvetoryDataToDB(FailedReason, inventoryList, dBStockDatas, productMasterList, dirName, StockDateStr, Date, InventoryPath);
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

     

    }
}
