using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using OMS.IDataAccessLayer_Muni;
using OMS.DataAccessLayer.Entity_Muni.EntityFrameWork_Muni;

namespace OMS.DataAccessLayer.Entity_Muni
{
    public class EntityEmployeeDAL:IEmployeeDAL
    {
        private static OMSEntities OMSDB {  get; set; }
        public EntityEmployeeDAL() { OMSDB = new OMSEntities(); }
        public void PushEmployeeDataToDB(List<EmployeeModel> employees)
        {
            foreach(var employee in employees)
            {
                var wareHouseIdpk = OMSDB.WareHouse.Where(x => x.WareHouseCode == employee.EmpWareHouseCode).Select(x => x.WareHouseIdpk).FirstOrDefault();
                if (OMSDB.Employee.Where(x=>x.EmpCode==employee.EmpCode) != null)
                {
                    Employee foundEmp = new Employee();
                    foundEmp.EmpName = employee.EmpName;
                    foundEmp.ContactNumber=employee.empContactNumber;
                    foundEmp.Salary=Convert.ToDecimal(employee.Salary);
                    foundEmp.Gender=employee.Gender;
                    foundEmp.WareHouseIdfk = Convert.ToInt32(wareHouseIdpk);
                    OMSDB.SaveChanges();

                }
                else
                {
                    Employee newEmp = new Employee();
                    newEmp.WareHouseIdfk = Convert.ToInt32(wareHouseIdpk);
                    newEmp.ContactNumber = employee.empContactNumber;
                    newEmp.Gender = employee.Gender;
                    newEmp.Salary = Convert.ToDecimal(employee.Salary);
                    newEmp.EmpCode = employee.EmpCode;
                    newEmp.EmpName= employee.EmpName;
                    OMSDB.Employee.Add(newEmp);
                    OMSDB.SaveChanges();
                }
            }


        }
    }
}
