using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    public class DBStockData
    {
        public int InventoryIdPk { get; set; }
        public int ProductIdFk { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int WareHouseIdFk { get; set; }
        public DateTime Date { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal RemainingQuantity {  get; set; }

    }
}
