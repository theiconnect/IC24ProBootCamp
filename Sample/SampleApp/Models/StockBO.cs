using System;

namespace SampleApp.Models
{
    internal class StockBO
    {
        public int StockIdPk { get; set; }
        public int ProductIdFk { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int StoreIdFk { get; set; }
        public DateTime Date { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
