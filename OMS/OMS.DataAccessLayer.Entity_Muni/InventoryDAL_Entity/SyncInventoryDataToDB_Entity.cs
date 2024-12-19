using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.DataAccessLayer.Entity_Muni.InventoryDAL_Entity
{
    internal class SyncInventoryDataToDB_Entity
    {
        public void SyncProducts(List<InventoryModel> inventoryList, List<ProductMasterModel> productMasterList)
        {
            foreach (var stock in inventoryList)
            {
                GetAllProductsFromDB();

                GetAllProductsFromDB();
                foreach (var eachStock in inventoryList)
                {
                    if (Convert.ToInt32(eachStock.ProductIdFk) == 0)
                    {
                        var eachProduct = productMasterList.FirstOrDefault(x => x.ProductCode == eachStock.productCode);
                        eachStock.ProductIdFk = eachProduct.ProductIdPk;
                    }
                }
            }


        }
        public List<DBStockData> GetAllStockInfoOfTodayFromDB(string StockDateStr)
        {
            List<DBStockData> dBStockDatas = new List<DBStockData>();
            
            return dBStockDatas;


        }
        public void SyncFileStockWithDB(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, string StockDateStr, string dirName)
        {
            foreach (var stock in inventoryList)
            {
                if (!dBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
                {
                    

                }

            }
        }
        public List<ProductMasterModel> GetAllProductsFromDB( )
        {
            List<ProductMasterModel> productMasterList = new List<ProductMasterModel>();
            return productMasterList;
            
        }
    }
}








