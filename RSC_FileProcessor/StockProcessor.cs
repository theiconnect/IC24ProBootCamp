using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;
using RSC_Configurations;
using RSC_DataAccess;
using RSC_IDAL;

namespace RSC_FileProcessor
{


    public class StockProcessor
    {
        private string stockFilePath { get; set; }
        private int storeid { get; set; }
        private string Storecode { get; set; }
        private string[] StockFileContant { get; set; }
        public List<Stockmodel> stocks {  get; set; }  
        private string FailReason { get; set; }
        private bool isValidFile { get; set; }  
        private IStockDAL StockObj { get; set; }

        public StockProcessor(string StockFilePath, int Storeid, string storeDirName, IStockDAL stockDALObj)
        {
            StockObj = stockDALObj;
            stockFilePath = StockFilePath;
            storeid = Storeid;
            Storecode = storeDirName;
        }
        public void Processor()
        {
            ReadStockData();
            ValidateDate();
            PrepareStockObject();
            StockDataAccess();
        }
        private void ReadStockData()
        {
            StockFileContant = File.ReadAllLines(stockFilePath);
            
        }
        private void ValidateDate()
        {
            StockFileContant = File.ReadAllLines(stockFilePath);

            if (StockFileContant.Length < 1)
            {
                FailReason = "log the error:invalid file";


            }
            else if (StockFileContant.Length == 1)
            {

                FailReason = "log the warning:no data found";
            }
            for (int i = 1; i < StockFileContant.Length; i++)
            {
                string[] StockData = StockFileContant[i].Split(';');

                if (StockData.Length != 6)
                {
                    FailReason = "Incorrect data came from stores  and send the correct data";
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                FileHelper.MoveFile(stockFilePath, FileStatus.Failure);
            }
            isValidFile = true; 
        }
        private void PrepareStockObject()
        {
            stocks = new List<Stockmodel>();
            for (int i = 1; i < StockFileContant.Length; i++)
            {
                string[] stockdata = StockFileContant[i].Split(';');
                Stockmodel model1 = new Stockmodel();
                model1.productCode =Convert.ToString( stockdata[0]);
                model1.storeidfk = storeid;
                model1.stockname =Convert.ToString( stockdata[2]);
                model1.QuantityAvailable = Convert.ToDecimal(stockdata[3]);
                model1.date = Convert.ToDateTime(stockdata[4]);
                model1.pricePerUint = Convert.ToDecimal(stockdata[5]);
                stocks.Add(model1);
                
            }
            
        }

        public void StockDataAccess()
        {
            if (!isValidFile)
            {
                return;
            }
            else
            {
                bool ISSuccess = StockObj.StockDBAcces(stocks, storeid);
                if (ISSuccess)
                {
                    FileHelper.MoveFile(stockFilePath, FileStatus.Success);
                }
                else
                {
                    FileHelper.MoveFile(stockFilePath, FileStatus.Failure);
                }
            }

        }

    }
}
