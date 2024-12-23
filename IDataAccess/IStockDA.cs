using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IStockDA
    {
        void GetrAllProductsFromDB(List<ProductMasterBO> products);
         void SyncStockTableData(List<StockBO> stockFileInformation);
        void SyncProductMasterTableData(List<ProductMasterBO> stockFileInformation);
    }
}
