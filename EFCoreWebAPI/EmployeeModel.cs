namespace EFCoreWebAPI
{
    public class EmployeeModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeIdPk { get; set; }
        public string StoreCode { get; set; }
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
