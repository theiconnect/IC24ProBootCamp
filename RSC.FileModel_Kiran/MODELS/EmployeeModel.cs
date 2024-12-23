using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.FileModel_Kiran
{ 
    
    public class EmployeeModel
    {
        public int employeesId { get; set; }
        public int Storeid { get; set; }    
        public  string employeeCode { get; set; }
        public  string employeeName { get; set; }
        public  string employeeRole { get; set; }
        public  DateTime employeeDateOfJoining { get; set; }
        public  DateTime employeeDateOfLeaving { get; set; }
        public  string employeeContactNumber { get; set; }
        public  string employeeGender { get; set; }
        public  decimal employeeSalary { get; set; }
        public List<EmployeeModel> employeeModels { get; set; } 

    }
}
