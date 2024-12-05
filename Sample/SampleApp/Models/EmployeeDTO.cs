using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Models
{
    internal class EmployeeDTO
    {
        public int EmployeeIdPk { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Role { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public decimal Salary { get; set; }
        public int StoreIdFk { get; set; }
    }
}
