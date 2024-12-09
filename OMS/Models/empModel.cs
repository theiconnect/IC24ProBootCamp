using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    internal class EmployeeModel
    {
        public int EmpIdpk { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpWareHouseCode { get; set; }
        public string empContactNumber { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
    }
}
