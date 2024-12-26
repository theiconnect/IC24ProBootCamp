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
        void SyncStockData(List<ProductMasterBO> products, List<StockBO> stockFileInformation, List<ProductMasterBO> StockFileInformation);
        bool SyncStockTableData(List<StockBO> stockFileInformation);


    }
}
