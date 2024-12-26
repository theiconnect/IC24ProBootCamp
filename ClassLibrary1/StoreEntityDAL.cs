using ClassLibrary1.EF;
using Models;
using RSC_IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_EntityDAL
{
    public class StoreEntityDAL:IstoreDA
    {
       
        private RSCEntities RSCDB { get; set; }

        public StoreEntityDAL()
        {
            RSCDB = new RSCEntities();
        }
        void IstoreDA.syncstoreTabledata(List<storemodel> store)
        {
            foreach (var stores in store)
            {
                var Store = RSCDB.Stores.FirstOrDefault(s => s.StoreCode == stores.StoreCode);
                if (Store != null) 
                {
                    Store.StoreName = stores.StoreName;
                    Store.Location = stores.Location;
                    Store.ContactNumber = stores.ContactNumber;
                    Store.ManagerName = stores.ManagerName;
                    RSCDB.SaveChanges();
                }
            }
           

        }
    }
}
