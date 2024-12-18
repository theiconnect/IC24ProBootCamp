using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_Models
{
    public class Stockmodel
    {
        public int stockidpk { get; set; }
        public string productCode { get; set; }
        public int storeidfk { get; set; }
        public string stockname { get; set; }
        public DateTime date { get; set; }
        public decimal QuantityAvailable { get; set; }
        public List<Stockmodel> storemodels { get; set; }
        public decimal pricePerUint { get; set; }
    }
}
