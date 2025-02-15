using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel.models
{
    public class InventoryPushData
    {
      
            public string FailedReason { get; set; }
            public List<InventoryModel> InventoryList { get; set; }
            public List<DBStockData> DBStockDatas { get; set; }
            public List<ProductMasterModel> ProductMasterList { get; set; }
            public string DirName { get; set; }
            public string StockDateStr { get; set; }
            public DateTime Date { get; set; }
        

    }
}
