using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;

namespace OMS.IDataAccessLayer_Muni.IWareHouseDAL
{
    internal interface IGetWareHousesDataFromDb
    {
        List<WareHouseModel> GetAllWareHouses();
    }
}
