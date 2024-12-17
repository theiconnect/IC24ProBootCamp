using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_v2
{
    internal class DBConstants
    {
        public const string GET_ALL_WAREHOUSES = "SELECT WarehouseIdPk,WarehouseCode,WarehouseName,Location,ManagerName,ContactNo FROM Warehouse ";
    }
}
