﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using OMS_Arjun_V3;

namespace Model
{ 
    public class EmployeeModel
    {
        public int EmpIdpk { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string WareHouseIdfk { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }


    }
}
