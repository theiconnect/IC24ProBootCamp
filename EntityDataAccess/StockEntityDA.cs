using EntityDataAccess.EF;
using IDataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDataAccess
{
    public class StockEntityDA:IStockDA
    {
        public RscEntities RSCDB { get; set; }
        public StockEntityDA() 
        { 
            RSCDB = new RscEntities();
        }
        public void SyncStockData(List<ProductMasterBO> products, List<StockBO> stockFileInformation, List<ProductMasterBO> StockFileInformation)
        {
            foreach(var stockData in stockFileInformation)
            {
                var dbProduct = RSCDB.ProductMaster.FirstOrDefault(p => p.ProductCode == stockData.ProductCode);
                
                if (dbProduct == null)
                {
                    ProductMaster productMaster = new ProductMaster();
                    productMaster.ProductIdPk = stockData.ProductIdPk;
                    productMaster.ProductName = stockData.ProductName;
                    productMaster.ProductCode = stockData.ProductCode;
                    productMaster.PricePerUnit = stockData.PricePerUnit;
                    RSCDB.ProductMaster.Add(productMaster);
                    RSCDB.SaveChanges();   
                }
            }
            
        }
        
        public  bool SyncStockTableData(List<StockBO> stockFileInformation)
        {
            foreach(var stock in stockFileInformation)
            {
                var StoreIdFK = RSCDB.Stores.Where(s=>s.StoreCode==stock.StoreCode).Select(s=>s.StoreIdPk).FirstOrDefault();
                var ProductIdFK=RSCDB.ProductMaster.Where(p=>p.ProductCode==stock.ProductCode).Select(s=>s.ProductIdPk).FirstOrDefault();

                if (RSCDB.Stock.FirstOrDefault(s => s.Date == stock.Date && s.ProductIdFk == ProductIdFK) ==null)
                {
                        Stock stockObj = new Stock();
                        
                        stockObj.StoreIdFk = StoreIdFK;
                        stockObj.ProductIdFk = ProductIdFK;
                        stockObj.QuantityAvailable = stock.QuantityAvailable;
                        stockObj.Date = stock.Date;
                        RSCDB.Stock.Add(stockObj);
                        RSCDB.SaveChanges();
                }
            }
            return true;
        }
    }
}
