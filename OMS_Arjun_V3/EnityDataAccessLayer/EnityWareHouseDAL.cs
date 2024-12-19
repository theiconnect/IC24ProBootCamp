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

namespace OMSEnityDataAccessLayer
{
    public class EnityWareHouseDAL:ConnectionConfig.ConnectionConfig1
    {

        public WareHouseModel wareHouseModel { get; set; }
        //private string[] WareHouseFileContent { get; set; }
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
