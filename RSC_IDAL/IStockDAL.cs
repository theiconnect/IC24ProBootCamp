using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_IDAL
{
   public interface IStockDAL
   {
        void SyncStockTableData(List<Stockmodel> stocks);


   }

}


    


    

