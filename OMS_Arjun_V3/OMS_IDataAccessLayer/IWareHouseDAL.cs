using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;
//using EnityDataAccessLayer.EntityFramework;
//using EnityDataAccessLayer;

namespace OMS_IDataAccessLayer
{
    public interface IWareHouseDAL
    {
         bool PushWareHouseDataToDB(WareHouseModel wareHouseModel);
       
         

        List<WareHouseModel> getAllWareHousesFromDB();
    }
}
