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
    
    public partial class OrderProduct
    {
        public int OrderProductsId { get; set; }
        public Nullable<int> OrderIdFk { get; set; }
        public Nullable<int> ProductIdFk { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Amount { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ProductsMaster ProductsMaster { get; set; }
    }
}
