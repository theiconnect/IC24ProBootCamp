using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSC_Models
{
    public class CustumerModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ContactNumber { get; set; }
        public bool IsValidCustomer { get; set; }

        public List<CustumerOrders> custumerOrders { get; set; }
    }
    public class CustumerOrders
    {
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public string StoreCode { get; set; }
        public string EmployeeCode { get; set; }
        public string ProductCode { get; set; }
        public DateTime OrderDatestr { get; set; }
        public DateTime OrderDate
        {
            get
            {
                return (Convert.ToDateTime(OrderDatestr));
            }
        }
        public int NoFoIteams { get; set; }
        public decimal Amount { get; set; }
        public bool IsValidOrder { get; set; }
        public List<BillingModel> OrderBillings { get; set; }
    }
    public class BillingModel
    {
        public string BillingNumber { get; set; }
        public string ModeOfPayment { get; set; }
        public string Ordercode { get; set; }
        public DateTime BillingDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsValidBilling { get; set; }

    }

}
