using ClassLibrary1.EF;
using Models;
using RSC_IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RSC_EntityDAL
{
    public class EmployeeEntityDAL:IemployeeDA
    {
        private int StoreId {  get; set; }  
        private RSCEntities RSCDB {  get; set; }

        public EmployeeEntityDAL()
        {
            RSCDB = new RSCEntities();  
        }
        void IemployeeDA.SyncEmployeeWithDB(List<EmployeeModel> empData, int storeid)
        {
            this.StoreId = storeid;

            foreach (var EmpData in empData)
            {
                var EmployeeData = RSCDB.Employees.FirstOrDefault(e => e.EmpCode == EmpData.EmpCode);
                if (EmployeeData != null)
                {
                    EmployeeData.EmployeeName = EmpData.EmployeeName;
                    EmployeeData.Salary = EmpData.Salary;
                    EmployeeData.Role = EmpData.Role;
                    EmployeeData.ContactNumber = EmpData.ContactNumber;
                    EmployeeData.DateOfJoining = EmpData.DateOfJoining;
                    EmployeeData.DateOfLeaving = EmpData.DateOfLeaving;
                    EmployeeData.Gender = EmpData.Gender;
                    EmployeeData.StoreIdFk = this.StoreId;
                    RSCDB.SaveChanges();
                }
                else
                {
                    Employee Empdata = new Employee();
                    Empdata.EmployeeName = EmpData.EmployeeName;
                    Empdata.Salary = EmpData.Salary;
                    Empdata.Role = EmpData.Role;
                    Empdata.ContactNumber = EmpData.ContactNumber;
                    Empdata.DateOfJoining = EmpData.DateOfJoining;
                    Empdata.DateOfLeaving = EmpData.DateOfLeaving;
                    Empdata.Gender = EmpData.Gender;
                    Empdata.StoreIdFk = this.StoreId;
                    RSCDB.Employees.Add(Empdata);
                    RSCDB.SaveChanges();
                }
            }
        }
    }
}
