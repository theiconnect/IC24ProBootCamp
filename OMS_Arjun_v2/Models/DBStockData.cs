using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_v2.Models
{
    internal class DBStockData
    {
        public int InventoryIdpk { get; set; }
        public int ProductIdfk { get; set; }        
        public int WarehouseIdfk { get; set; }
        public DateTime Date { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal RemainingQuantity { get; set; }
    }
}
