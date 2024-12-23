using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;
using RSC_IDAL;
using RSCEntityDAL;
using RSCEntityDSAL.EF;

namespace RSCEntityDSAL
{
    public class StoreEntityDAL:IStoreDAL
    {
        private RSCEntities RSCDB {  get; set; } 

        public StoreEntityDAL()
        {
            RSCDB = new RSCEntities();  
        }

        public void PushStoreDataToDB(List<StoreModel> Model)
        {
            foreach (var model in Model)
            {
                var stores = RSCDB.Stores.FirstOrDefault(x=>x.StoreCode == model.storeCode);
                if (stores != null)
                {
                    stores.Location = model.location;
                    stores.ContactNumber = model.contactNumber;
                    stores.StoreName = model.storeName;
                    stores.ManagerName = model.managerName;
                }
            }
        }
        
    }
}


     