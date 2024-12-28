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
using FileModel.models;

namespace FileProcesses
{
    public class InventoryProcess : BaseProcessor
    {

        private string InventoryPath { get; set; }
        private string FailedReason { get; set; }
        private DateTime Date { get; set; }
        private string StockDateStr { get; set; }

        private bool IsValidFile { get; set; } = true;
        private IInventoryDAL ObjInventoryDal { get; set; }
        List<InventoryModel> inventoryList { get; set; }
        List<ProductMasterModel> productMasterList { get; set; }
        List<DBStockData> dBStockDatas { get; set; }

        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(InventoryPath)); }
        }
        public InventoryProcess(string inventorypath, IInventoryDAL objInventoryDal)
        {

            InventoryPath = inventorypath;
            ObjInventoryDal = objInventoryDal;
        }

        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            if (!IsValidFile) 
            {
                FileHelper.MoiveFile(InventoryPath, Enum.FileStatus.Failure);
                return;
            }

            var listOfParameter=new InventoryPushData
            {
                FailedReason = FailedReason,
                InventoryList = inventoryList,
                DBStockDatas = dBStockDatas,
                ProductMasterList = productMasterList,
                DirName = dirName,
                StockDateStr = StockDateStr,
                Date = Date
            };
            bool isSucess= ObjInventoryDal.PushInvetoryDataToDB(listOfParameter);
            if (isSucess)
            {
                FileHelper.MoiveFile(InventoryPath, Enum.FileStatus.Success);
            }
            else
            {
                FileHelper.MoiveFile(InventoryPath, Enum.FileStatus.Failure);

            }

        }


        private void ReadFileData()
        {
            try
            {
                using (StreamReader reader = new StreamReader(InventoryPath))
                {
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

                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because it doesn't have any records in this file.please check and update the file.\n");
            }
            else if (inventoryList.Count == 1)
            {
                FailedReason = "Log the warning: No data present in the file";
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because it doesn't have any records in this file.please check and update the file.\n");
            }
            else
            {
                foreach (var data in inventoryList)
                {
                    if (data.wareHouseCode != dirName)
                    {
                        FailedReason = "log the error : the record doesn't matches the current warehosue code";
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because the warehouse code in the file doesn't match respective directory/warehouse. productCode:{data.productCode} & Date:{data.date}.please check and update the file.\n");

                    }
                    if (!DateTime.TryParse(data.date.ToString(), out DateTime date))
                    {
                        FailedReason += "log the error : the record doesn't have valid date";
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because the date is not in correct format.productCode:{data.productCode}.please check and update the file.\n");
                    }
                    if (!decimal.TryParse(data.availableQuantity.ToString(), out decimal result))
                    {
                        FailedReason += $"Error: invalid quantity avaiable data; value:{data.availableQuantity};recordNumber:{data};";
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because invalid quantity avaiable data.productCode:{data.productCode};recordNumber:{data};.please check and update the file.\n");
                    }
                    if (!decimal.TryParse(data.pricePerUnit.ToString(), out decimal price))
                    {
                        FailedReason += $"Error: invalid price; value:{data.pricePerUnit};recordNumber:{data};";
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because invalid Price data. productCode:{data.productCode};recordNumber:{data};.please check and update the file.\n");
                    }
                    if (!decimal.TryParse(data.remainingQuantity.ToString(), out decimal rem))
                    {
                        FailedReason += $"Error: invalid data; value:{data.remainingQuantity};recordNumber:{data} ;";
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code {dirName} is not a valid file because invalid remaining Quantity avaiable data. productCode:{data.productCode};recordNumber:{data};.please check and update the file.");
                    }

                    if (!string.IsNullOrEmpty(FailedReason))
                    {
                        break;
                    }
                    Date = Convert.ToDateTime(data.date);
                    StockDateStr = Date.ToString("yyyy-MM-dd");
                }



            }
            if (!string.IsNullOrEmpty(FailedReason))
            {
                IsValidFile = false;
            }

        }

     

    }
}
