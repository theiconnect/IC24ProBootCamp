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
        public void GetrAllProductsFromDB(List<ProductMasterBO> products)
        {
              
            var dbProducts=RSCDB.ProductMaster.ToList();
            foreach (var product in dbProducts) 
            {
                ProductMasterBO productMasterObj=new ProductMasterBO();
                productMasterObj.ProductIdPk = product.ProductIdPk;
                productMasterObj.ProductName = product.ProductName;
                productMasterObj.ProductCode= product.ProductCode;
                productMasterObj.PricePerUnit = product.PricePerUnit;
                products.Add(productMasterObj);

            }

        }
        public void SyncStockTableData(List<StockBO> stockFileInformation)
        {
            foreach(var stock in stockFileInformation)
            {
                Stock stockObj=new Stock();
                stockObj.StockIdPk = stock.StockIdPk;
                stockObj.StoreIdFk = stock.StoreIdFK;
                stockObj.ProductIdFk = stock.ProductIdFk;
                stockObj.QuantityAvailable=stock.QuantityAvailable;
                stockObj.Date=stock.Date;
                RSCDB.Stock.Add(stockObj);
                RSCDB.SaveChanges();



            }



        }
        public void SyncProductMasterTableData(List<ProductMasterBO> stockFileInformation)
        {
            
            foreach(var stockInformation in stockFileInformation)
            {

                var dbStock = RSCDB.ProductMaster.FirstOrDefault(p => p.ProductCode == stockInformation.ProductCode);
                if(dbStock == null)
                {
                    ProductMaster productMaster = new ProductMaster();
                    productMaster.ProductIdPk = stockInformation.ProductIdPk;
                    productMaster.ProductName= stockInformation.ProductName;
                    productMaster.ProductCode= stockInformation.ProductCode;
                    productMaster.PricePerUnit= stockInformation.PricePerUnit;
                    RSCDB.ProductMaster.Add(productMaster);
                    RSCDB.SaveChanges();


                }




            }
            



        }
    }
}
