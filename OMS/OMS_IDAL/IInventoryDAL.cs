using FileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_IDAL
{
    public interface IInventoryDAL
    {

        void PushInvetoryDataToDB(string failedReason, List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList, string dirName, string stockDateStr, DateTime date, string inventoryPath);



        void SyncProducts(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList);


        List<ProductMasterModel> GetAllProductsFromDB(List<ProductMasterModel> productMasterList);

        List<DBStockData> GetAllStockInfoOfTodayFromDB(List<DBStockData> dBStockDatas, DateTime date);

        void SyncStockFileWithDB(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, string dirName, string stockDateStr, string inventoryPath);
       
    }
}
