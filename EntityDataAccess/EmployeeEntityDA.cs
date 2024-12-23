using EntityDataAccess.EF;
using IDataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDataAccess
{
    public class EmployeeEntityDA:IEmployeeDA
    {
        public RscEntities rscEntities { get; set; }
        public EmployeeDTO employee {  get; set; }
        public EmployeeEntityDA() 
        {
            rscEntities = new RscEntities();
        }

        public void SyncEmployeeData(List<EmployeeDTO> employeeData, List<EmployeeDTO> fileEmployeeDTOObject)
        {

        }
        public void GetAllEmployeeDataFromDB(List<EmployeeDTO> employeeData)
        {
            List<EmployeeDTO> employeeDTOObj= new List<EmployeeDTO>();
            var employees=rscEntities.Employee.ToList();
            foreach (var employee in employees)
            { 
                EmployeeDTO employeeObj = new EmployeeDTO();
                employeeObj.EmployeeName=employee.EmployeeName;
                employeeObj.EmployeeCode=employee.EmployeeCode;
                employeeObj.ContactNumber=employee.ContactNumber;
                employeeObj.Role=employee.Role;
                employeeObj.DateOfJoining=Convert.ToDateTime(employee.DateOfJoining);
                employeeObj.DateOfLeaving = Convert.ToDateTime(employee.DateOfLeaving);
                employeeObj.Salary = Convert.ToDecimal(employee.salary);
                employeeObj.Gender = employee.gender;
                employeeDTOObj.Add(employeeObj);

            }



        }
        public void SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject)
        {
            
                       // List<EmployeeDTO> employeeList = new List<EmployeeDTO>();
           foreach(var employee in fileEmployeeDTOObject) 
           {
                var dbEmployee = rscEntities.Employee.FirstOrDefault(E => E.EmployeeCode == employee.EmployeeCode);

                if (dbEmployee == null)
                {

                    Employee emp =new Employee();
                    emp.EmployeeCode = employee.EmployeeCode;
                    emp.EmployeeName = employee.EmployeeName;
                    emp.ContactNumber = employee.ContactNumber;
                    emp.DateOfJoining = this.employee.DateOfJoining;
                    emp.DateOfLeaving = this.employee.DateOfLeaving;
                    emp.salary = Convert.ToString(this.employee.Salary);
                    emp.Role = this.employee.Role;
                    emp.gender = this.employee.Gender;
                    emp.StoreIdFk = this.employee.StoreIdFk;

                    rscEntities.Employee.Add(emp);
                    rscEntities.SaveChanges();



                }
                else
                {

                    dbEmployee.EmployeeCode = this.employee.EmployeeCode;
                    dbEmployee.EmployeeName = this.employee.EmployeeName;
                    dbEmployee.ContactNumber = this.employee.ContactNumber;
                    dbEmployee.DateOfJoining = this.employee.DateOfJoining;
                    dbEmployee.DateOfLeaving = this.employee.DateOfLeaving;
                    dbEmployee.salary = Convert.ToString(this.employee.Salary);
                    dbEmployee.Role = this.employee.Role;
                    dbEmployee.gender = this.employee.Gender;
                    dbEmployee.StoreIdFk = this.employee.StoreIdFk;
                    rscEntities.SaveChanges();



                }

            }

            


        }
    }
}
