//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OMSEntityDAL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int EmpIdpk { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int WareHouseIdfk { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public decimal Salary { get; set; }
    
        public virtual WareHouse WareHouse { get; set; }
    }
}
