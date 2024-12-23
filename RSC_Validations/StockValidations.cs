using DataBaseAccessLayer;
using RSC.FileModel_Kiran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RSC.FileModel_Kiran;
using System.Collections;


namespace RSC_Validations
{
    public class StockValidations
    {
        private string[] StockFileContant { get; set; }
        public int StoreId { get; set; }
        public string FailReason { get; set; }
        public void ValidateDate(string[] stockFileContant, int storeid)
        {
            this.StockFileContant = stockFileContant; 
            this.StoreId = storeid;

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
            PrepareStockObject();
        }
        private void PrepareStockObject()
        {
            List<Stockmodel> Stocks = new List<Stockmodel>();
            Stockmodel model1 = new Stockmodel();
            for (int i = 1; i < StockFileContant.Length; i++)
            {
                string[] stockdata = StockFileContant[i].Split(';');
                model1.ProductCode = stockdata[0];
                model1.Storeidfk = StoreId;
                model1.StockName = stockdata[2];
                model1.QuantityAvailable = Convert.ToDecimal(stockdata[3]);
                model1.date = Convert.ToDateTime(stockdata[4]);
                model1.pricePerUint = Convert.ToDecimal(stockdata[5]);
                Stocks.Add(model1);
                StocksDataPushToDB obj = new StocksDataPushToDB();
                obj.SyncStockTableData(Stocks);
            }
        }

    }

}
    





    

