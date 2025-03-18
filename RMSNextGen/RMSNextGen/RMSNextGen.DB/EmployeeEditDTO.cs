using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class EmployeeEditDTO
	{
		public int EmployeeidPK {  get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeFirstName { get; set; }
		public string EmployeeLastName { get; set; }
		public string Email { get; set; }
		public string MobileNumber { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		//public int StoreIdFk { get; set; }
		//public int StatusIdFk { get; set; }
		public string PersonalEmail { get; set; }
		public string Gender { get; set; }
		public int Salary { get; set; }
		public string PermanentAddressLine1 { get; set; }
		public string PermanentAddressline2 { get; set; }
		public string PermanentCity { get; set; }
		public string PermanentState { get; set; }
		public int PermanentPincode { get; set; }
		public string CurrentAddressline1 { get; set; }
		public string CurrentAddressline2 { get; set; }
		public string CurrentCity { get; set; }
		public string CurrentState { get; set; }
		public int CurrentPincode { get; set; }
		//public string CreatedBy { get; set; }
		//public DateTime CreatedOn { get; set; }
		//public string LastUpdatedBy { get; set; }
		//public DateTime LastUpdatedOn { get; set; }


	}
}
