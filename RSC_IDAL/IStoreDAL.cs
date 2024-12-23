using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;

namespace RSC_IDAL
{
    public interface  IStoreDAL
    {
        bool StoreDBAcces( List<StoreModel> storeModels);
    }
}
