//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RSC_EntityDAL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int StockIdPk { get; set; }
        public Nullable<int> ProductIdFk { get; set; }
        public Nullable<int> StoreIdFk { get; set; }
        public System.DateTime Date { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal PricePerUnit { get; set; }
    
        public virtual ProductsMaster ProductsMaster { get; set; }
        public virtual Store Store { get; set; }
    }
}