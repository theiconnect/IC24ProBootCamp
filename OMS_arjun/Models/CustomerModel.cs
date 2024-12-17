using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun.Models;

namespace OMS_arjun
{
    internal class CustomerModel
    {
        public string CustomerName { get; set; }
        public int CustomerIdPk { get; set; }
        public string ContactNumber { get; set; }
        public List<OrdersModel> Orders { get; set; }
    }
}
