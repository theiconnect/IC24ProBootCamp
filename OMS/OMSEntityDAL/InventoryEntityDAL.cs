using Enum;
using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_IDAL;

namespace OMSEntityDAL
{
    public class InventoryEntityDAL:IInventoryDAL
    {

        public void PushInvetoryDataToDB(string failedReason, List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList, string dirName, string stockDateStr, DateTime date, string inventoryPath)
        {

            if (!string.IsNullOrEmpty(failedReason))
            {
                return;
            }

            SyncProducts(inventoryList, dBStockDatas, productMasterList);

            dBStockDatas = GetAllStockInfoOfTodayFromDB(dBStockDatas, date);

            SyncStockFileWithDB(inventoryList, dBStockDatas, dirName, stockDateStr, inventoryPath);


        }
        public   void SyncProducts(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList)
        {

            productMasterList = GetAllProductsFromDB(productMasterList);
          
        }

        public List<ProductMasterModel> GetAllProductsFromDB(List<ProductMasterModel> productMasterList)
        {
            return null;
        }

        public List<DBStockData> GetAllStockInfoOfTodayFromDB(List<DBStockData> dBStockDatas, DateTime date)
        {
            return null;

        }

        public void SyncStockFileWithDB(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, string dirName, string stockDateStr, string inventoryPath)
        {
           
        }

    }
}
