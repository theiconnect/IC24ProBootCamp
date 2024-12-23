using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.FileModel_Kiran
{
    public class Stockmodel
    {
        public  int StockId { get; set; }
        public  int Storeidfk { get; set; }
        public string ProductCode { get; set; }
        public string StockName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public  DateTime date { get; set; }
        public decimal pricePerUint { get; set; }
       
        public static List<Stockmodel> Stocks { get; set; }

    }
}
