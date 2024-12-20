using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.IDataAccessLayer_Muni.IWareHouseDAL
{
    internal interface ISyncWareHouseDataToDB
    {
        void PushWareHouseDataToDB(WareHouseModel wareHouseModel);
        
    }
}
