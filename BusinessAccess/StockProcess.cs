
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;
namespace RSC
{
    public class StockProcess
    {
        private string StockFilePath { get; set; }
        private int StoreIdFk { get; set; }
        private string[] stockFileContent { get; set; }
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private string[] stockFileData { get; set; }
        private string storeDirName { get { return Path.GetFileName(Path.GetDirectoryName(StockFilePath)); } }
        private List<StockBO> stockFileInformation { get; set; }
        private List<StockBO> stockDBData { get; set; }
        private List<ProductMasterBO> Products { get; set; }
        

        public StockProcess(string stockFilePath, int storeIdFk)
        {
            StockFilePath = stockFilePath;
            StoreIdFk = storeIdFk;
        }



        public void Process()
        {
            ReadFileData();
            ValidateStockData();
            PushStockDataToDB();
            FileHelper.Move(StockFilePath, FileStatus.Sucess);


        }

        private void ReadFileData()
        {
            stockFileContent = File.ReadAllLines(StockFilePath);
        }

        private void ValidateStockData()
        {
            if (stockFileContent.Length < 1)
            {
                isValidFile = false;//this line no need to write because bool default value is false
                FailReason = "Log the error: Invalid file";

            }
            else if (stockFileContent.Length == 1)
            {
                isValidFile = false;//this line no need to write because bool default value is false
                FailReason = "Log the warning: No data present in the file.";
            }
            else
            {
                for (int i = 1; i < stockFileContent.Length; i++)
                {
                    stockFileData = stockFileContent[i].Split(';');



                    if (stockFileContent.Length != 8)
                    {
                        FailReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 8.";
                        break;
                    }
                    if (stockFileData[1].ToLower() != storeDirName.ToLower())
                    {
                        FailReason = "Log the error: Invalid data; store code not matching with current storecode.";

                    }
                    if (!decimal.TryParse(stockFileData[3], out decimal availableQunatity))
                    {
                        FailReason += $"Error:Invalid quantity available data;value={stockFileData[3]};recordNumber:{i};";

                    }
                    if (!DateTime.TryParse(stockFileData[4], out DateTime date))
                    {
                        FailReason += $"Errror:Invalid Date; value:{stockFileData[4]};recordNumber:{i}; ";

                    }
                    if (!decimal.TryParse(stockFileData[5], out decimal price))
                    {
                        FailReason += $"Error: invalid price; value:{stockFileData[5]};recordNumber:{i} ;";

                    }


                    if (!string.IsNullOrEmpty(FailReason))
                        break;
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                //I am cheking any FailReason is their or not and failreason is their means invalid file
                isValidFile = false;

                return;
            }
            isValidFile = true;



        }


        private void PushStockDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            prepareStockObject();
            StockDA stockObj = new StockDA();
            stockObj.SyncStockData(Products, stockFileInformation);
        }
        private void prepareStockObject()
        {
            stockFileInformation = new List<StockBO>();
            for (int i = 1; i < stockFileContent.Length; i++)
            {

                stockFileData = stockFileContent[i].Split(';');
                StockBO stockModelObject = new StockBO();

                stockModelObject.ProductCode = stockFileData[0];
                stockModelObject.StoreCode = stockFileData[1];
                stockModelObject.ProductName = stockFileData[2];
                if (decimal.TryParse(stockFileData[3], out decimal availableQuantity))
                    stockModelObject.QuantityAvailable = availableQuantity;
                if (DateTime.TryParse(stockFileData[4], out DateTime date))
                    stockModelObject.Date = date;
                if (decimal.TryParse(stockFileData[5], out decimal price))
                    stockModelObject.PricePerUnit = price;

                stockFileInformation.Add(stockModelObject);
            }
        }
    }
}


