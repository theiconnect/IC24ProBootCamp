using FileModel;
using FileModel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_IDAL
{
    public interface IInventoryDAL
    {

        bool PushInvetoryDataToDB(InventoryPushData data);




       
    }
}
