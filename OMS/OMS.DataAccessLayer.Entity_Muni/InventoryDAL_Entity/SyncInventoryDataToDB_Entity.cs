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
        public void SyncProducts(InventoryModel stock)
        {            
            
        }
        public List<ProductMasterModel> GetAllProductsFromDB()
        {
            return null;
           
        }
        public List<DBStockData> GetAllStockInfoOfTodayFromDB(string StockDateStr)
        {
            return null;
            
        }
        public void SyncFileStockWithDB(InventoryModel stock, string StockDateStr, string dirName)
        {
           

        }
    }
}








