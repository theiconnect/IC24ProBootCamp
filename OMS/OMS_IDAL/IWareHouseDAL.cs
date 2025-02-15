using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
namespace OMS_IDAL
{
    public interface IWareHouseDAL
    {
        List<WareHouseModel> GetAllWareHouses();
        bool PushWareHouseDataToDB(WareHouseModel wareHouseModel);
    }
}
