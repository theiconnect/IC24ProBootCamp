using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SampleApp.Models
{
    internal class CustomerModel
    {
        public int CustomerIdPk { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public List<CustomerOrderModel> customerOrders { get; set; }
    }

    internal class CustomerOrderModel
    {
        public string OrderCode { get; set; }
        public int OrderIdPk { get; set; }
        public int StoreIdFk { get; set; }
        public int CustomerIdFk { get; set; }
        public int EmployeeIdFk { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal NoOfItems { get; set; }
        public decimal Amount { get; set; }
        public List<OrderBillingModel> billingModels { get; set; }
    }

    internal class OrderBillingModel
    {
        public int BillIdPk { get; set; }
        public string BillNumber { get; set; }
        public int OrderIdFk { get; set; }
        public string PaymentMode { get; set; }
        public DateTime BillingDate { get; set; }
        public decimal Amount { get; set; }
    }
}
