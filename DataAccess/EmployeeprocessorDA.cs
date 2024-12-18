using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataBaseConfig;

namespace DataAccess
{
    public  class EmployeeprocessorDA
    {

        public static  void SyncEmployeeWithDB(List<EmployeeModel> empData)
        {
            foreach (var employeedata in empData)
            {
                using (SqlConnection con = new SqlConnection(ConfigHelper.connectionString))
                {
                    string Query = $"if exists(select * from Employees where EmpCode='{employeedata.EmpCode}')\r\nbegin\r\nupdate Employees\r\nset EmployeeName='{employeedata.EmployeeName}',Role='{employeedata.Role}',DateOfJoining='{employeedata.DateOfJoiningstr}',\r\nDateOfLeaving={employeedata.DateOfLeavingstr},ContactNumber='{employeedata.ContactNumber}',Gender='{employeedata.Gender}',Salary={employeedata.Salary}\r\nwhere EmpCode='{employeedata.EmpCode}'\r\nend\r\nelse\r\nbegin\r\nInsert into Employees (EmpCode,EmployeeName,Role,DateOfJoining,DateOfLeaving,ContactNumber,Gender,Salary,StoreIdFk)\r\nvalues('{employeedata.EmpCode}','{employeedata.EmployeeName}','{employeedata.Role}','{employeedata.DateOfJoiningstr}',{employeedata.DateOfLeavingstr},'{employeedata.ContactNumber}','{employeedata.Gender}',{employeedata.Salary})\r\nend";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
