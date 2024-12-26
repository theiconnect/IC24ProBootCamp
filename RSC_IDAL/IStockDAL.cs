using RSC_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_IDAL
{
    public interface IStockDAL
    {
        void NewStockUpdateInProductMaster(List<Stockmodel> stocks);
        bool StockDBAcces(List<Stockmodel> stocks, int storeid);
    }
}
