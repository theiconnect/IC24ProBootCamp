using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductMasterBO
    {
        public int ProductIdPk { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
