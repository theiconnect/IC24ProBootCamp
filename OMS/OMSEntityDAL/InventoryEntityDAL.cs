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


            SyncStockFileWithDB(pushData);
            return true;

        }
        public   void SyncProducts(InventoryPushData pushData)
        {
            foreach(var item in pushData.InventoryList)
            {
                var product = OMSEntities.Products.FirstOrDefault(x => x.ProductCode == item.productCode);
                if (product != null)
                {
                    //Here We need to add the Product details into product table
                }


            }

        }

        public void SyncStockFileWithDB(InventoryPushData pushData)
        {
           foreach(var stock in pushData.InventoryList)
            {
                var producIdFk=OMSEntities.Products.Where(x=>x.ProductCode==stock.productCode).Select(x=>x.ProductIdpk).FirstOrDefault(); 
                var wareHouseIdFk=OMSEntities.WareHouse.Where(x=>x.WareHouseCode==stock.wareHouseCode).Select(x=>x.WareHouseIdpk).FirstOrDefault(); 
                if(OMSEntities.Inventory.FirstOrDefault(x=>x.Date  == stock.date && x.ProductIdfk== producIdFk) == null)
                {
                    Inventory inventory = new Inventory();
                    inventory.ProductIdfk= producIdFk;
                    inventory.WarehouseIdfk = wareHouseIdFk;
                    inventory.Date=stock.date;
                    inventory.AvailableQuantity=stock.availableQuantity;
                    inventory.PricePerUnit=stock.pricePerUnit;
                    inventory.RemainingQuantity=stock.remainingQuantity;
                    OMSEntities.Inventory.Add(inventory);
                    OMSEntities.SaveChanges();
                }
            }
        }

    }
}
