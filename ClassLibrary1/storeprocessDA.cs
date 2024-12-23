using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using DataBaseConfig;
using System.Data;
using System.Reflection;
using DataBaseConfig;
using RSC_IDAL;


namespace RSC_Entity
{
    public class storeprocessDA : ConfigHelper, IstoreDA
    {
        public  List<storemodel> GetAllStoresFromDB()

        {

            return null;
        }
      
        
        public  void syncstoreTabledata(storemodel modelObj)

        {

                      
        }
    }
}
