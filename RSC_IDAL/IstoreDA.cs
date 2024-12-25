using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RSC_IDAL
{
    public interface IstoreDA
    {
        List<storemodel> GetAllStoresFromDB();

        void syncstoreTabledata(storemodel modelObj);
    }
   
}


