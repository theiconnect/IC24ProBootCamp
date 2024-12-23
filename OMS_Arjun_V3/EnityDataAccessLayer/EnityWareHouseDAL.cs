using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;
using System.Runtime.Remoting.Contexts;
using EnityDataAccessLayer.EntityFramework;
using OMS_IDataAccessLayer;

namespace OMSEnityDataAccessLayer
{
    public class EnityWareHouseDAL : ConnectionConfig.ConnectionConfig1,IWareHouseDAL
    {

        public WareHouseModel wareHouseModel { get; set; }
        public string[] WareHouseFileContent { get; set; }
        Arjun_OMSDBEntities1 Arjun_OMSDB { get; set; }
        public EnityWareHouseDAL()
        {
            Arjun_OMSDB=new Arjun_OMSDBEntities1();
        }

       

        public  List<WareHouseModel> getAllWareHousesFromDB()
        {
            List<WareHouseModel> wareHouseModels = new List<WareHouseModel>();
            var warehouses = Arjun_OMSDB.WareHouses.ToList();
            foreach (var WareHouse in warehouses)
            {
                wareHouseModels.Add(new WareHouseModel
                {
                    WareHouseIdpk = WareHouse.WareHouseIdpk,
                    WareHouseName = WareHouse.WareHouseName,
                    WareHouseCode = WareHouse.WareHouseCode,
                    ContactNo = WareHouse.ContactNo,
                    ManagerName = WareHouse.ManagerName,
                    Location = WareHouse.Location,
                });
            }
            return wareHouseModels;
        }
        public bool PushWareHouseDataToDB(WareHouseModel wareHouseModel)
        {
            try
            {
                var dbWh = Arjun_OMSDB.WareHouses.FirstOrDefault(w => w.WareHouseCode == wareHouseModel.WareHouseCode);
                if (dbWh == null)
                {
                    Console.Write($"file warehouse code:{wareHouseModel.WareHouseCode} didn't match with the db records.");
                    return false;
                }

                dbWh.ContactNo = wareHouseModel.ContactNo;
                dbWh.Location = wareHouseModel.Location;
                dbWh.ManagerName = wareHouseModel.ManagerName;
                dbWh.WareHouseName = wareHouseModel.WareHouseName;

                
                Arjun_OMSDB.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void prepareWareHouseObject(string[] wareHouseFileContent)
        {
            wareHouseModel = new WareHouseModel();

            string[] data = WareHouseFileContent[1].Split('|');
            wareHouseModel.WareHouseCode = data[0];
            wareHouseModel.WareHouseName = data[1];
            wareHouseModel.Location = data[2];
            wareHouseModel.ManagerName = data[3];
            wareHouseModel.ContactNo = data[4];
        }

       
    }
}
