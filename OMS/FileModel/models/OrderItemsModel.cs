﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    public class OrderItemsModel
    {

        public int OrderItemIdPk { get; set; }
        public string InvoiceNumber {  get; set; }
        public string WareHouseCode {  get; set; }
        public int WareHosueIdFk { get; set; }

        public string ProductCode { get;set; }
        public int ProductIdFk { get; set; }
        public decimal Quantity {  get; set; }
        public decimal PricePerUnit { get; set; }
         public decimal TotalAmount {  get; set; }
        public bool IsValidItem { get; set; } = true;
    }
}
