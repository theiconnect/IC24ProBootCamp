namespace RMSNextGen.Web.Models
{
	public class EmployeeViewModel
	{
		public string EmployeeCode { get; set; }
		public string EmployeeFirstName { get; set; }
		public string EmployeeLastName { get; set; }
		public string Email { get; set; }
		public string MobileNumber { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public int StoreIdFk { get; set; }
		public int StatusIdFk { get; set; }
		public string PersonalEmail { get; set; }
		public string Gender { get; set; }
		public string SalaryCTC { get; set; }
		public string PermanentAddressline1 { get; set; }
		public string PermanentAddressline2 { get; set; }
		public string PermanentCity { get; set; }
		public string PermanentState { get; set; }
		public string PermanentPincode { get; set; }
		public string CurrentAddressline1 { get; set; }
		public string CurrentAddressline2 { get; set; }
		public string CurrentCity { get; set; }
		public string CurrentState { get; set; }
		public string CurrentPincode { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public string LastUpdatedBy { get; set; }
		public DateTime LastUpdatedOn { get; set; }

	}
}
