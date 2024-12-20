using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDataAccess;
using Models;

namespace EntityDataAccess
{
    public class StoreEntityDA:IStoreDA
    {
        public  List<StoreModel> GetAllStoresDataFromDB()
        {
            return null;
        }
        public void SyncStoreDataToDB(StoreModel storeModelObject)
        {

        }
    }
}
