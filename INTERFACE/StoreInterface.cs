using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.FileModels_Venky;

namespace INTERFACE
{
    public interface StoreInterface
    {
        void PushStoreDataToDB(StoreModel Model);
    }
}
