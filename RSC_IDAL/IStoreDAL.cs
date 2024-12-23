using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;

namespace RSC_IDAL
{
    public interface IStoreDAL
    {
        void PushStoreDataToDB(List<StoreModel> Stores);
    }
}


     
    

