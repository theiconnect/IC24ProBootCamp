using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using OMS_Arjun_V3;

namespace Model
{
    public class ProductModel
    {
        public int ProductIdPk { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public int CategoryIdfk {  get; set; }

        
    }
}
