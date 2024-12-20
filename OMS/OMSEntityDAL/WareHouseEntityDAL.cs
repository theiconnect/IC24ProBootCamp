using FileModel;
using OMS_IDAL;
using OMSEntityDAL.EF;
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
                    Console.Write($"file warehouse code:{wareHouseModel.WareHouseCode} didn't match with the db records.");
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
                return false;
            }
        }
    }
}
