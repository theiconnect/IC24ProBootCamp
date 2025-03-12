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
        public RscEntities RSCDB{ get; set; }        
        public EmployeeEntityDA() 
        {
            RSCDB = new RscEntities();
        }
        public bool SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject)
        {
            foreach (var employee in fileEmployeeDTOObject)
            {
                var dbEmployee = RSCDB.Employee.FirstOrDefault(E => E.EmployeeCode == employee.EmployeeCode);
                var StoreIdFk = RSCDB.Stores.Where(s => s.StoreCode == employee.StoreCode).Select(s => s.StoreIdPk).FirstOrDefault();
                if (dbEmployee == null)
                {
                    Employee emp = new Employee();
                    emp.EmployeeCode = employee.EmployeeCode;
                    emp.EmployeeName = employee.EmployeeName;
                    emp.ContactNumber = employee.ContactNumber;
                    emp.DateOfJoining = employee.DateOfJoining;
                    emp.DateOfLeaving = employee.DateOfLeaving;
                    emp.salary = Convert.ToString(employee.Salary);
                    emp.Role = employee.Role;
                    emp.gender = employee.Gender;
                    emp.StoreIdFk = StoreIdFk;
                    RSCDB.Employee.Add(emp);
                    RSCDB.SaveChanges();

                }
                else
                {
                    dbEmployee.EmployeeCode = employee.EmployeeCode;
                    dbEmployee.EmployeeName = employee.EmployeeName;
                    dbEmployee.ContactNumber = employee.ContactNumber;
                    dbEmployee.DateOfJoining = employee.DateOfJoining;
                    dbEmployee.DateOfLeaving = employee.DateOfLeaving;
                    dbEmployee.salary = Convert.ToString(employee.Salary);
                    dbEmployee.Role = employee.Role;
                    dbEmployee.gender = employee.Gender;
                    dbEmployee.StoreIdFk = StoreIdFk;
                    RSCDB.SaveChanges();

                }
            } 
            return true;

        }
        
    }
}
