using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmployeeDTO
    {

        //EmployeeCode|StoreCode|EmployeeName|Role|DateOfJoining|DateOfLeaving|ContactNumber|Gender|Salary|StoreId
        //Emp001|STHYD001|mani|SrManager|2023-4-12|Null|7834598765|Male|50,000|1
        public  string EmployeeCode { get; set; }
        public  string EmployeeIdPk { get; set; }
        public  string StoreCode { get; set; }
        public  string EmployeeName { get; set; }
        public  string Role { get; set; }
        public  DateTime DateOfJoining { get; set; }
        public  DateTime? DateOfLeaving { get; set; }
        public  string ContactNumber { get; set; }
        public  string Gender { get; set; }
        public  decimal Salary { get; set; }
        public  int StoreIdFk { get; set; }


    }
}
