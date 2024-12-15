using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileModel
{
    public class InventoryModel
    {
        public  string wareHouseCode{get; set;}
        public  int ProductIdFk { get; set;}
        public string productCode { get; set;}
        public DateTime date {  get; set;}
        public decimal availableQuantity {  get; set;}
        public decimal pricePerUnit { get; set;}
        public decimal remainingQuantity { get; set;}

    }
}
