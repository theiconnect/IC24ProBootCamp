using RSC_Configurations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;
using RSC_FileProcessor;
using RSC_IDAL;
using RSC_EntityDAL.EF;
using RSC_Models;

namespace RSC_EntityDAL
{
    public class EmployeeEntityDAL:IEmployeeDAL
    {
        private RSCEntities RSCDB {  get; set; }   
        
        public EmployeeEntityDAL()
        {
            RSCDB = new RSCEntities();
        }
        private int Storeid { get; set; }
        public bool EmployeeDBAcces(List<EmployeeModel> empData, int storeid)
        {
            Storeid = storeid;

           foreach (var emp in empData)
           {
                var EmpData = RSCDB.Employees.FirstOrDefault(x=> x.EmpCode == emp.EmpCode);
                if (EmpData != null)
                {
                    EmpData.EmployeeName = emp.EmployeeName;
                    EmpData.Role = emp.Role;    
                    EmpData.Gender = emp.Gender;    
                    EmpData.Salary = emp.Salary;    
                    EmpData.DateOfJoining = emp.DateOfJoining;  
                    EmpData.DateOfLeaving = emp.DateOfLeaving;
                    EmpData.ContactNumber = emp.ContactNumber;
                    EmpData.StoreIdFk = this.Storeid;
                    RSCDB.SaveChanges();    
                }
                else
                {
                    Employees EmpData1 = new Employees();
                    EmpData1.EmpCode = emp.EmpCode;  
                    EmpData1.EmployeeName = emp.EmployeeName;
                    EmpData1.Role = emp.Role;
                    EmpData1.Gender = emp.Gender;
                    EmpData1.Salary = emp.Salary;
                    EmpData1.DateOfJoining = emp.DateOfJoining;
                    EmpData1.DateOfLeaving = emp.DateOfLeaving;
                    EmpData1.ContactNumber = emp.ContactNumber;
                    EmpData1.StoreIdFk = this.Storeid;
                    RSCDB.Employees.Add(EmpData1);
                    RSCDB.SaveChanges();
                }
            }
           return true;
        }
    }
}
