using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_v2.Models
{
    internal class InventoryModel
    {
        public int InventoryIdpk { get; set; }
        public string WarehouseIdfk { get; set; }
        public int ProductIdFk { get; set; }
        public DateTime date { get; set; }
        public decimal availableQuantity { get; set; }
        public decimal pricePerUnit { get; set; }
        public decimal Remianingquantity { get; set; }

        
    }
}
