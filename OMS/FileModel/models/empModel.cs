using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    public class EmployeeModel
    {
        public int EmpIdpk { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpWareHouseCode { get; set; }
        public bool IsValidEmpolyee { get; set; } = true;
        public string EmpContactNumber { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
    }
}
