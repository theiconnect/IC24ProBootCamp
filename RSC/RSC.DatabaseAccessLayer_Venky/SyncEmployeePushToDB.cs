
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.Configuration_Venky;
using RSC.FileModels_Venky;


namespace RSC.DatabaseAccessLayer_Venky 
{
    internal class SyncEmployeePushToDB:AppConnection
    {
        private void PushStoreDataToDB(EmployeeModel model)
        {
           
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                string query = $"Update Employees Set =EmployeeCode=@EmployeeCode,EmployeeName=@EmployeeName,EmployeeRole=@EmployeeRole,EmployeeDateOfJoining=@EmployeeDateOfJoining,EmployeeDateOfLeaving=@employeeDateOfleaving,EmployeeContactNumber=@EmployeeContactNumber,EmployeeGender=@EmployeeGender,EmployeeSalary=@EmployeeSalary,";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@EmployeeCode", DbType.String).Value = model.employeeCode;
                    cmd.Parameters.Add("@EmployeeName", DbType.String).Value = model.employeeName;
                    cmd.Parameters.Add("@EmployeeRole", DbType.String).Value = model.employeeRole;
                    cmd.Parameters.Add("@EmployeeDateOfJoining", DbType.String).Value = model.employeeDateOfJoining;
                    cmd.Parameters.Add("@EmployeeDateOfleaving", DbType.String).Value = model.employeeDateOfLeaving;
                    cmd.Parameters.Add("@EmployeeContactNumber", DbType.String).Value = model.employeeContactNumber;
                    cmd.Parameters.Add("@EmployeeGender", DbType.String).Value = model.employeeGender;
                    cmd.Parameters.Add("@EmployeeSalary", DbType.String).Value = model.employeeSalary;

                    //Employees.add(model);
                    con.Close();

                }
            }
        }





    }
}
