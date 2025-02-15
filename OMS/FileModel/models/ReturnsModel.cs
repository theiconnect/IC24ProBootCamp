using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    public class ReturnsModel
    {
        public string InvoiceNumber{get;set;}
        public string WareHouseCode {get;set;}
        public string ReturnStatus{get;set;}
        public string Reason{get;set;}

        public bool IsvalidReturn { get; set; } = true;
        public decimal AmountRefund{get;set;}
        public string Date{get;set; }
    }
}
