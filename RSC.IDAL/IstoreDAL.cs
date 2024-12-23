using RSC.FileModels_Venky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.IDAL
{
   public interface IstoreDAL
    {
        void PushStoreDataToDB(StoreModel Model);
    }
}
