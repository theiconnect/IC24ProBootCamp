using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.FileModels_Venky
{ 
    
    public class EmployeeModel
    {
        public string employeesId { get; set; }
        public  string employeeCode { get; set; }
        public  string employeeName { get; set; }
        public  string employeeRole { get; set; }
        public  string employeeDateOfJoining { get; set; }
        public  string employeeDateOfLeaving { get; set; }
        public  string employeeContactNumber { get; set; }
        public  string employeeGender { get; set; }
        public  string employeeSalary { get; set; }
        public List<EmployeeModel> employeeModels { get; set; } 

    }
}
