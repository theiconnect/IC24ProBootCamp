//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnityDataAccessLayer.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Return
    {
        public int ReturnIdpk { get; set; }
        public int OrderIdfk { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string Reason { get; set; }
        public int ReturnStatusIdfk { get; set; }
        public Nullable<decimal> AmountRefund { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ReturnStatu ReturnStatu { get; set; }
    }
}