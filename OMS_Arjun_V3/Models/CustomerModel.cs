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
        public string CustomerIdpk { get; set; }
        public int CustomerName { get; set; }
        public string PhNo { get; set; }
        public List<OrdersModel> Orders { get; set; }

       
    }
}
