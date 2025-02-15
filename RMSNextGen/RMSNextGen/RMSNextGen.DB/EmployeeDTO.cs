using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class EmployeeDTO
	{
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string StoreCode { get; set; }
		public string Role { get; set; }
		public string Gender { get; set; }
		public string Salary { get; set; }
		public string ContactNumber { get; set; }

		public string Addressline1 { get; set; }
		public string Addressline2 { get; set; }

		public string City { get; set; }
		public string State { get; set; }
		public string Pincode { get; set; }
	}
}
