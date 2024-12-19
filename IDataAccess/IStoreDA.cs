using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IStoreDA
    {
         List<StoreModel> GetAllStoresDataFromDB();
         void SyncStoreDataToDB(StoreModel storeModelObject);
    }
}
