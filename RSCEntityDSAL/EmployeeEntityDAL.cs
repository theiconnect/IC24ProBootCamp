using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_IDAL;
using RSCEntityDSAL.EF;
using RSCEntityDAL;




namespace RSCEntityDAL
{
    public class EmployeeEntityDAL : IEmployeeDAL
    {
        private RSCEntities RSCDB { get; set; }
        
        public EmployeeEntityDAL()
        {
            RSCDB = new RSCEntities();
        }
        public void PushStoreDataToDB(List<EmployeeModel> Model, int storeId)
        {
            foreach (var model in Model)
            {
                var employees = RSCDB.Employees.FirstOrDefault(x => x.EmpCode == model.employeeCode);
                if (employees != null)
                {
                  employees.EmployeeName = model.employeeName;
                  employees.ContactNumber = model.employeeContactNumber;
                  employees.DateOfJoining=model.employeeDateOfJoining;
                  employees.DateOfLeaving=model.employeeDateOfLeaving;
                  employees.Role=model.employeeRole;
                  employees.Gender=model.employeeGender;
                  employees.StoreIdFk = storeId;
                  employees.Salary=model.employeeSalary;
                }
            }
        }
    }

}

