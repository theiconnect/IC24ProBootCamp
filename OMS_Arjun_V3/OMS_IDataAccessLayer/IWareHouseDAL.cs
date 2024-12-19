using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;

namespace OMS_IDataAccessLayer
{
    public class IWareHouseDAL
    {
        public void UpdateWareHouseDataToDB(string[] WareHouseFileContent, string WareHouseFilePath)
        {

        }
        public void prepareWareHouseObject(string[] wareHouseFileContent)
        {

        }
        public static List<WareHouseModel> getAllWareHousesFromDB()
        {
            var wareHouses = new List<WareHouseModel>();
            return wareHouses;
        }
    }
}
