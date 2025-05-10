using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CustomerModel
    {
        public int CustomerIdPk { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public bool IsValidCustomer { get; set; }
        public List<CustomerOrderModel> CustomerOrders { get; set; }



    }
    public class CustomerOrderModel
    {
        public int OrderIdPk { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerIdFk { get; set; }
        public string StoreCode { get; set; }
        public int StoreIdFk { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeIdFk { get; set; }

        public string ProductCode { get; set; }
        public int ProductIdFk { get; set; }

        public DateTime OrderDate { get; set; }
        public int NoOfItems { get; set; }
        public decimal Amount { get; set; }
        public List<OrderBillingModel> OrderBilling { get; set; }

        public bool IsValidOrder { get; set; }

    }
    public class OrderBillingModel
    {
        public int BillingIdPk { get; set; }
        public string BillingNumber { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerIdFk { get; set; }
        public int OrderIdFk { get; set; }
        public DateTime BillingDate { get; set; }
        public string ModeOfPayment { get; set; }
        public decimal Amount { get; set; }
        public bool IsValidBilling { get; set; }
    }
}
