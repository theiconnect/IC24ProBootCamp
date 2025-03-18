using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StockBO
    {
        //ProductCode;StoreCode;ProductName;QuantityAvailable;Date;PricePerUnit
        //PRD001;STHYD001;sugar;20;28-11-2024;50.00
        public int StockIdPk { get; set; }
        public int ProductIdFk { get; set; }
        public string ProductCode { get; set; }
        public int StoreIdFK { get; set; }
        public string StoreCode { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public DateTime Date { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
