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

namespace RSC_FileProcessor
{


    public class StockProcessor
    {
        private string stockFilePath { get; set; }
        private int storeid { get; set; }
        private string Storecode { get; set; }
        private string[] StockFileContant { get; set; }

        private string FailReason { get; set; }

        public StockProcessor(string StockFilePath, int Storeid, string storeDirName)
        {
            stockFilePath = StockFilePath;
            storeid = Storeid;
            Storecode = storeDirName;
        }
        public void Processor()
        {
            ReadStockData();
            ValidateDate();
        }

        private void ReadStockData()
        {
            string[] StockFileContant = File.ReadAllLines(stockFilePath);
            List<Stockmodel> stocks = new List<Stockmodel>();
            Stockmodel model1 = new Stockmodel();
            for (int i = 1; i < StockFileContant.Length; i++)
            {
                string[] stockdata = StockFileContant[i].Split(';');

                ///var product = products.FirstOrDefault(x => x.ProductCode == stockdata[0]);
                model1.productCode = stockdata[0];
                model1.storeidfk = storeid;
                model1.stockname = stockdata[2];
                model1.QuantityAvailable = Convert.ToDecimal(stockdata[3]);
                model1.date = Convert.ToDateTime(stockdata[4]);
                model1.pricePerUint = Convert.ToDecimal(stockdata[5]);
                stocks.Add(model1);
                new StockDBAccess(stocks);
            }
        }
        private void ValidateDate()
        {

            if (StockFileContant.Length < 1)
            {
                FailReason = "log the error:invalid file";


            }
            else if (StockFileContant.Length == 1)
            {

                FailReason = "log the warning:no data found";
            }
            else if (StockFileContant.Length > 2)
            {
                FailReason = "log the error: invalid file has multiple store records";

            }
            for (int i = 1; i < StockFileContant.Length; i++)
            {
                string[] StockData = StockFileContant[i].Split(';');

                if (StockData.Length != 6)
                {
                    FailReason = "Incorrect data came from stores  and send the correct data";
                }
            }
        }

    }
}
