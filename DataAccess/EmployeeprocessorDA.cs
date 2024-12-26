using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataBaseConfig;
using System.Collections;
using System.Data;
using System.Reflection;
using RSC_IDAL;


namespace DataAccess
{
    public  class EmployeeprocessorDA: IemployeeDA
    {
        public int StoreIdFk {  get; set; }
       public void  SyncEmployeeWithDB(List<EmployeeModel> empData, int storeid)
        {
            foreach (var employeedata in empData)
            {
                using (SqlConnection con = new SqlConnection(ConfigHelper.connectionString))
                {
                    string Query = "update Employees\r\n\t\tset EmployeeName=@employeename,\r\n\t\t\tRole=@role,\r\n\t\t\tDateOfJoining=@dateofjoining,\r\n\t\t\tDateOfLeaving=@dateofleaving,\r\n\t\t\tContactNumber=@contactnumber,\r\n\t\t\tGender=@gender,\r\n\t\t\tSalary=@salary,\r\n\t\t\tStoreIdFk=@storeidfk\r\n\t\t\twhere EmpCode=@empcode";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                       
                            cmd.CommandText = Query;
                            cmd.Connection = con;
                            cmd.Parameters.Add("@empcode", DbType.String).Value = employeedata.EmpCode;
                            cmd.Parameters.Add("@EmployeeName", DbType.String).Value = employeedata.EmployeeName;
                            cmd.Parameters.Add("@role", DbType.String).Value = employeedata.Role;
                            cmd.Parameters.Add("@dateofjoining", DbType.String).Value = employeedata.DateOfJoining;
                            cmd.Parameters.Add("@dateofleaving", DbType.String).Value = employeedata.DateOfLeaving;
                            cmd.Parameters.Add("@contactnumber", DbType.String).Value = employeedata.ContactNumber;
                            cmd.Parameters.Add("@gender", DbType.String).Value = employeedata.Gender;
                            cmd.Parameters.Add("@salary", DbType.String).Value = employeedata.Salary;
                            cmd.Parameters.Add("@storeidfk", DbType.String).Value = this.StoreIdFk;
                            //Employees.add(model);
                            con.Close();
                            cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
