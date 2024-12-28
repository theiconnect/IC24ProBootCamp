using FileModel;
using System.Collections.Generic;
using OMS_IDAL;
using FileModel.models;
using OMSEntityDAL.EF;
using System.Linq;
using System;
using ProjectHelpers;

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


            
            return SyncStockFileWithDB(pushData); 

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

        public bool SyncStockFileWithDB(InventoryPushData pushData)
        {
            try
            {
                var count = 0;
                foreach (var stock in pushData.InventoryList)
                {
                    var producIdFk = OMSEntities.Products.Where(x => x.ProductCode == stock.productCode).Select(x => x.ProductIdpk).FirstOrDefault();
                    var wareHouseIdFk = OMSEntities.WareHouse.Where(x => x.WareHouseCode == stock.wareHouseCode).Select(x => x.WareHouseIdpk).FirstOrDefault();


                    if (OMSEntities.Inventory.FirstOrDefault(x => x.Date == stock.date && x.ProductIdfk == producIdFk) == null)
                    {
                        Inventory inventory = new Inventory();
                        inventory.ProductIdfk = producIdFk;
                        inventory.WarehouseIdfk = wareHouseIdFk;
                        inventory.Date = stock.date;
                        inventory.AvailableQuantity = stock.availableQuantity;
                        inventory.PricePerUnit = stock.pricePerUnit;
                        inventory.RemainingQuantity = stock.remainingQuantity;
                        OMSEntities.Inventory.Add(inventory);
                        OMSEntities.SaveChanges();
                        count++;
                    }


                }
                FileHelper.LogEntries($"[{DateTime.Now}] INFO: The Inventory file which is  associated with the warehouse code {pushData.DirName} is suceesfully processed and the file is moved into processed folder.Rows affeceted:{count} \n");
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Inventory file which is  associated with the warehouse code:{pushData.DirName} is invalid file and got the exception {ex.Message}.Please check and update the file. \n");
                return false;
            }

        }

    }
}
