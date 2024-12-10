using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models
{
    internal class CustomerModel
    {
        public string CustomerName { get; set; }
        public int CustomerIdPk { get; set; }
        public string ContactNumber { get; set; }
        public List<OrdersModel> Orders { get; set; }

    }
}
