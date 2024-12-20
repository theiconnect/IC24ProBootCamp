using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.IDataAccessLayer_Muni.IInventoryDAL
{
    internal interface ISyncInventoryFileDataToDB
    {
        void SyncProducts(InventoryModel stock);

        List<ProductMasterModel> GetAllProductsFromDB();

        List<DBStockData> GetAllStockInfoOfTodayFromDB(string StockDateStr);

        void SyncFileStockWithDB(InventoryModel stock, string StockDateStr, string dirName);

    }
}
