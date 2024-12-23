using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS.IDataAccessLayer_Muni;
using OMS.DataAccessLayer.Entity_Muni.EntityFrameWork_Muni;

namespace OMS.DataAccessLayer.Entity_Muni
{
    public class EntityInventoryDAL:IInventoryDAL
    {
        private OMSEntities OMSDB { get; set; }
        public EntityInventoryDAL()
        {
            OMSDB = new OMSEntities();
        }
        public void SyncProducts(InventoryModel stock)
        {
            Products newProduct = new Products();
            newProduct.ProductCode=stock.productCode;
            newProduct.ProductName=stock.productCode;
            newProduct.PricePerUnit=stock.pricePerUnit;
            newProduct.CategoryIdfk = 1;
            OMSDB.Products.Add(newProduct);
            OMSDB.SaveChanges();
            
        }
        public List<ProductMasterModel> GetAllProductsFromDB()
        {
            List<ProductMasterModel> products= new List<ProductMasterModel>();
            var productRecords = OMSDB.Products;
            foreach (var record in productRecords) 
            { 
                ProductMasterModel product= new ProductMasterModel();
                product.ProductName=record.ProductName;
                product.ProductCode=record.ProductCode;
                product.PricePerUnit=record.PricePerUnit;
                product.ProductIdPk=record.ProductIdpk;
                products.Add(product);
            }
            return products;
           
        }
        public List<DBStockData> GetAllStockInfoOfTodayFromDB(string StockDateStr)
        {
            List<DBStockData> dBStockData = new List<DBStockData>();
            var inventoryStock = OMSDB.Inventory.Where(x=>x.Date==Convert.ToDateTime(StockDateStr));
            foreach (var record in inventoryStock) 
            {
                var product = OMSDB.Products.Where(x => x.ProductIdpk == record.ProductIdfk).FirstOrDefault();
                DBStockData stock=new DBStockData();
                stock.QuantityAvailable=record.AvailableQuantity;
                stock.RemainingQuantity=record.RemainingQuantity;
                stock.PricePerUnit = record.PricePerUnit;
                stock.InventoryIdPk=record.InventoryIdpk;
                stock.WareHouseIdFk =record.WarehouseIdfk;
                stock.ProductCode =product.ProductCode;
                stock.ProductName =product.ProductName;
                stock.ProductIdFk = record.ProductIdfk;
                stock.InventoryIdPk = record.InventoryIdpk;
                dBStockData.Add(stock);

            }
            return dBStockData;
            
        }
        public void SyncFileStockWithDB(InventoryModel stock, string StockDateStr, string dirName)
        {
           var warehouseIdfk=OMSDB.WareHouse.Where(x=>x.WareHouseCode== dirName).Select(x=>x.WareHouseIdpk).FirstOrDefault();
            
           Inventory newInventory = new Inventory();
           newInventory.ProductIdfk = stock.ProductIdFk;           
           newInventory.WarehouseIdfk = warehouseIdfk;
           newInventory.PricePerUnit=stock.pricePerUnit;
           newInventory.AvailableQuantity=stock.availableQuantity;
           newInventory.Date= Convert.ToDateTime(StockDateStr);
           newInventory.RemainingQuantity=stock.remainingQuantity;
           OMSDB.SaveChanges();

        }
    }
}








