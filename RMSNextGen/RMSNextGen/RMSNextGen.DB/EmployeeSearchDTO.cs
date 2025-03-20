using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class EmployeeSearchDTO
    {
        public EmployeeSearchDTO(string EmployeeName, string EmployeeCode, string MobileNumber, int DepartmentId)
        {
            this.EmployeeName = EmployeeName;
            this.EmployeeCode = EmployeeCode;
            this.MobileNumber = MobileNumber;
            this.DepartmentId = DepartmentId;
        }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string MobileNumber { get; set; }
        public int DepartmentId { get; set; }
    }
}
