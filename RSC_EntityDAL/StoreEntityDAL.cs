using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Configurations;
using RSC_Models;
using RSC_FileProcessor;
using RSC_IDAL;
using RSC_EntityDAL.EF;

namespace RSC_EntityDAL
{
    public class StoreEntityDAL:IStoreDAL
    {
        private RSCEntities RSCDB {  get; set; }  
        public StoreEntityDAL()
        {
            RSCDB = new RSCEntities();         
        }
        public bool StoreDBAcces(List<StoreModel> storeModels)
        {
            foreach (var model in storeModels)
            {
                var store = RSCDB.Stores.FirstOrDefault(s => s.StoreCode == model.StoreCode);
                if (store != null)
                {
                    store.StoreName = model.StoreName;
                    store.Location = model.Location;    
                    store.ContactNumber = model.ContactNumber;
                    store.ManagerName = model.ManagerName;
                    RSCDB.SaveChanges();
                }
            }
             return true;
        }
    }
}
