using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class EmployeeListDTO
	{
		public int EmployeeID { get; set; }
		//public int EmployeeIdPk { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string EmployeeFirstName { get; set; }
		public string EmployeeLastName { get; set; }
		public string Gender { get; set; }
	
		public string Designation { get; set; }
		public string MobileNumber { get; set; }
		public string StoreIdFk { get; set; }
		
	}
}
