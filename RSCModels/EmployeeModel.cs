using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSCModels
{
    public class EmployeeModel
    {

        public int EmployeeIdPK { get; set; }
        public int EmployeeIdPk { get; set; }
        public int StoreIdFk { get; set; }
        public string EmployeeCode { get; set; }
        public string StoreCode { get; set; }
        public string EmployeeName { get; set; }
        public string Role { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public decimal Salary { get; set; }
        public int EmpCode { get; set; }
    }
}
