using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS.IDataAccessLayer_Muni;
using FileProcessses;
using OMS.DataAccessLayer.Entity_Muni.EntityFrameWork_Muni;

namespace OMS.DataAccessLayer.Entity_Muni
{
    public class EntityWareHouseDAL:IWareHouseDAL
    {
        private OMSEntities OMSDB {  get; set; }
        public EntityWareHouseDAL()
        {
            OMSDB = new OMSEntities();
        }
        public  List<WareHouseModel> GetAllWareHouses()
        {
            List<WareHouseModel> wareHouseModels = new List<WareHouseModel>();
            var warehouses=OMSDB.WareHouse.ToList();
            foreach(var warehouse in warehouses)
            {
                wareHouseModels.Add(new WareHouseModel
                {
                    WareHouseidpk = warehouse.WareHouseIdpk,
                    ContactNumber = warehouse.ContactNo,
                    Location = warehouse.Location,
                    ManagerName = warehouse.ManagerName,
                    WareHouseCode = warehouse.WareHouseCode,
                    WareHouseName = warehouse.WareHouseName
                });
            }
            return wareHouseModels;

        }
        public void PushWareHouseDataToDB(WareHouseModel wareHouseModel)
        {
            var warehouseFound =OMSDB.WareHouse.FirstOrDefault(W=>W.WareHouseCode==wareHouseModel.WareHouseCode);
            if (warehouseFound!=null) 
            { 
                warehouseFound.ContactNo=wareHouseModel.ContactNumber;
                warehouseFound.Location=wareHouseModel.Location;
                warehouseFound.ManagerName=wareHouseModel.ManagerName;
                warehouseFound.WareHouseName=wareHouseModel.WareHouseName;
                OMSDB.SaveChanges();
            }

        }
    }
}
