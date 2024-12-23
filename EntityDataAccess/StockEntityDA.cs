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
        public RscEntities rscEntities { get; set; }
        public StockEntityDA() 
        { 
            rscEntities= new RscEntities();
        }
        public void GetrAllProductsFromDB(List<ProductMasterBO> products)
        {
            List<ProductMasterBO> allProducts = products.ToList();  
            var dbProducts=rscEntities.ProductMaster.ToList();
            foreach (var product in allProducts) 
            {
                ProductMasterBO productMasterObj=new ProductMasterBO();
                productMasterObj.ProductIdPk = product.ProductIdPk;
                productMasterObj.ProductName = product.ProductName;
                productMasterObj.ProductCode= product.ProductCode;
                productMasterObj.PricePerUnit = product.PricePerUnit;
                allProducts.Add(productMasterObj);

            }

        }
        public void SyncStockTableData(List<StockBO> stockFileInformation)
        {


        }
        public void SyncProductMasterTableData(List<StockBO> stockFileInformation)
        {


        }
    }
}
