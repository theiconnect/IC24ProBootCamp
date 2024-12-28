using FileModel;
using OMS_IDAL;
using OMSEntityDAL.EF;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSEntityDAL
{
    public class WareHouseEntityDAL : IWareHouseDAL
    {
        OMSEntities OMSDB { get; set; }
        public WareHouseEntityDAL()
        {
            OMSDB = new OMSEntities();
        }
        public List<WareHouseModel> GetAllWareHouses()
        {
            List<WareHouseModel> wareHouseModels = new List<WareHouseModel>();

            var warehouses = OMSDB.WareHouse.ToList();

            foreach (var warehouse in warehouses)
            {
                wareHouseModels.Add(new WareHouseModel
                {
                    WareHouseidpk = warehouse.WareHouseIdpk,
                    ContactNumber = warehouse.ContactNo,
                    Location = warehouse.Location,
                    ManagerName = warehouse.ManagerName,
                    WareHouseCode = warehouse.WareHouseCode,
                    WareHouseName   = warehouse.WareHouseName
                });
            }

            //foreach (var wh in warehouses)
            //{
            //    wh.Location += "_iConnect";
            //}
            //OMSDB.WareHouse.Remove(warehouses[6]);
            //OMSDB.SaveChanges();

            return wareHouseModels;
        }

        public bool PushWareHouseDataToDB(WareHouseModel wareHouseModel)
        {
            try
            {
                var dbWh = OMSDB.WareHouse.FirstOrDefault(w => w.WareHouseCode == wareHouseModel.WareHouseCode);
                if(dbWh == null)
                {
                    FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The warehouse file code does not match the warehouse code {wareHouseModel.WareHouseCode}\n");
                    return false;
                }

                dbWh.ContactNo = wareHouseModel.ContactNumber;
                dbWh.Location = wareHouseModel.Location;
                dbWh.ManagerName = wareHouseModel.ManagerName;
                dbWh.WareHouseName = wareHouseModel.WareHouseName;
                
                //OMSDB.WareHouse.Add(warehouse);
                OMSDB.SaveChanges();
                return true;
                 
            }
            catch (Exception ex)
            {
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR:The Warehouse file which is  associated with the warehouse code {wareHouseModel.WareHouseCode} is not a valid file.got {ex.Message} and  the file is moved to error folder. Please check and update the file\n");
                return false;
            }
        }
    }
}
