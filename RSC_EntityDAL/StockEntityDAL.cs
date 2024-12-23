using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Configurations;
using RSC_Models;
using RSC_FileProcessor;
using RSC_IDAL;
using RSC_EntityDAL.EF;

namespace RSC_EntityDAL
{
    public class StockEntityDAL:IStockDAL
    {
        private RSCEntities RSCDB {  get; set; }  
        private int Storeid { get; set; }   

        public StockEntityDAL()
        {
            RSCDB = new RSCEntities();
        }
        public bool StockDBAcces(List<Stockmodel> stocks, int storeid)
        {
            this.Storeid = storeid; 
            foreach (var stock in stocks)
            {
                var productID = RSCDB.ProductsMasters.Where(p => p.ProductCode == stock.productCode)
                                                     .Select(p=>p.ProductIdPk).FirstOrDefault();
                var stock1 = RSCDB.Stocks.FirstOrDefault(s=>s.ProductIdFk == productID);
                if(stock1 != null)
                {
                    stock1.PricePerUnit = stock.pricePerUint;
                    stock1.QuantityAvailable = stock.QuantityAvailable;
                    stock1.Date = stock.date;
                    stock1.StoreIdFk = this.Storeid;
                    RSCDB.SaveChanges();    
                }
                else
                {
                    Stock StockData = new Stock();
                    StockData.ProductIdFk = productID;
                    StockData.StoreIdFk = this.Storeid;
                    StockData.Date = stock.date;
                    StockData.PricePerUnit = stock.pricePerUint;
                    StockData.QuantityAvailable = stock.QuantityAvailable;
                    RSCDB.Stocks.Add(StockData);
                    RSCDB.SaveChanges();
                }
            }
            return true;
        }
    }
}
