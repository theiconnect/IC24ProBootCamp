using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    internal class OrdersModel
    {

        public string OrderIdPk {  get; set; }
        public string InvoiceNumber {  get; set; }
        public string WareHouseCode {  get; set; }
        public int WareHouseIdFk {  get; set; }
        public string CustomerPhNo { get; set; }
        public DateTime Date {  get; set; }
        public decimal NoOfItems {  get; set; }

        public string PaymentStaus { get; set; }
        public decimal TotalAmount {  get; set; }
        public int CustomerIdFk { get; set; }
        public bool IsValidOrder {  get; set; }=true;
        public List<OrderItemsModel> Items { get; set; }

    }
}
