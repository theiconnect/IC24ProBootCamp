using FileModel;
using System.Collections.Generic;
using OMS_IDAL;
using FileModel.models;
using OMSEntityDAL.EF;
using System.Linq;

namespace OMSEntityDAL
{
    public class InventoryEntityDAL:IInventoryDAL
    {
        OMSEntities OMSEntities { get; set; }
        public InventoryEntityDAL()
        {
            OMSEntities=new OMSEntities();
        }
        public bool PushInvetoryDataToDB(InventoryPushData pushData)
        {
            SyncProducts(pushData);

            pushData.DBStockDatas = GetAllStockInfoOfTodayFromDB(pushData);

            SyncStockFileWithDB(pushData);
            return true;

        }
        public   void SyncProducts(InventoryPushData pushData)
        {

            pushData.ProductMasterList = GetAllProductsFromDB(pushData);
            var product = OMSEntities.Products.FirstOrDefault(x => x.ProductCode ==);

        }

        public List<ProductMasterModel> GetAllProductsFromDB(InventoryPushData pushData)
        {

            pushData.ProductMasterList=new List<ProductMasterModel>();
            foreach (var productRecord in OMSEntities.Products)
            {
                ProductMasterModel productMasterModel = new ProductMasterModel();

                productMasterModel.ProductIdPk=productRecord.ProductIdpk;
                productMasterModel.ProductName=productRecord.ProductName;
                productMasterModel.ProductCode=productRecord.ProductCode;
                productMasterModel.PricePerUnit=productRecord.PricePerUnit;
                pushData.ProductMasterList.Add(productMasterModel);


            }
            return pushData.ProductMasterList;
        }

        public List<DBStockData> GetAllStockInfoOfTodayFromDB(InventoryPushData pushData)
        {
            return null;

        }

        public void SyncStockFileWithDB(InventoryPushData pushData)
        {
           
        }

    }
}
