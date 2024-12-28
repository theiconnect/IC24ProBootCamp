using FileModel;
using OMS_IDAL;
using OMSEntityDAL.EF;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSEntityDAL
{
    public class EmployeeEntityDAL:IEmployeeyDAL
    {
        OMSEntities OMSEntities { get; set; }
        public EmployeeEntityDAL()
        {
            OMSEntities = new OMSEntities();
        }
        public  bool PushEmployeeDataToDB(List<EmployeeModel> EmployeesList)
        {

            try
            {
                var count = 0;
                foreach (var EmployeeModel in EmployeesList)
                {
                    var employee = OMSEntities.Employee.FirstOrDefault(x => x.EmpCode == EmployeeModel.EmpCode);
                    var warehosueidFk = OMSEntities.WareHouse.Where(x=> x.WareHouseCode==EmployeeModel.EmpWareHouseCode).Select(x => x.WareHouseIdpk).FirstOrDefault();
                    if (employee == null)
                    {
                        
                       
                       Employee emp=new Employee();
                        emp.WareHouseIdfk = warehosueidFk;
                        emp.EmpCode = EmployeeModel.EmpCode;
                        emp.EmpName = EmployeeModel.EmpName;
                        emp.Gender = EmployeeModel.Gender;
                        emp.Salary = Convert.ToDecimal(EmployeeModel.Salary);
                        emp.ContactNumber=EmployeeModel.EmpContactNumber;
                        OMSEntities.Employee.Add(emp);
                        OMSEntities.SaveChanges();
                        count++;
                    }
                    else
                    {
                       
                        employee.EmpCode = EmployeeModel.EmpCode;
                        employee.EmpName = EmployeeModel.EmpName;
                        employee.ContactNumber = EmployeeModel.EmpContactNumber;
                        employee.WareHouseIdfk = Convert.ToInt32(warehosueidFk);
                        employee.Salary = Convert.ToDecimal(EmployeeModel.Salary);
                        employee.Gender = EmployeeModel.Gender;

                        OMSEntities.SaveChanges();
                        count++;


                    }
                }
                FileHelper.LogEntries($"[{DateTime.Now}] INFO: The Employee file which is  associated with the warehouse code {EmployeesList.Select(x => x.EmpWareHouseCode).FirstOrDefault()} is successfully processed  and the file is moved to processed folder. Record got affected:{count}\n");
                return true;
            }
            catch (Exception ex)
            {
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Employee file which is  associated with the warehouse code {EmployeesList.Select(x => x.EmpWareHouseCode).FirstOrDefault()} is not a valid file.got an error '{ex.Message}' and  the file is moved to error folder. Please check and update the file \n");
                return false;

            }

        }
    }

}
