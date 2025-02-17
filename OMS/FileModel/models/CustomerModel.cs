using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    public  class CustomerModel
    {
        public string CustomerName {  get; set; }
        public int CustomerIdPk {  get; set; }
        public string ContactNumber {  get; set; }
        public bool IsValidCustomer { get; set; } = true;
        public List<OrdersModel> Orders { get; set; }

    }
}
