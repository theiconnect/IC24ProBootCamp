using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_arjun.Models
{
    internal class OrdersModel
    {
        public string OrderIdPk { get; set; }
        public string InvoiceNumber { get; set; }
        public int WareHouseIdFk { get; set; }
        public string CustomerPhNo { get; set; }
        public DateTime Date { get; set; }
        public decimal NoOfItems { get; set; }
        public string PaymentStatusIdfk { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerIdFk { get; set; }
        public bool IsValidRecord { get; set; } = true;
        public List<OrderItemsModel> Items { get; set; }

        //OrderIdpk	WareHouseIdfk	CustomerIdfk	OrderDate	InvoiceNumber	NoOfItems	PaymentStatusIdfk	TotalAmount

    }
}
