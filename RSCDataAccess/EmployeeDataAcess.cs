using RSCConfigiration;
using RSCModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RSCIDataAcess;


namespace RSCDataAccess

{
  
    public class EmployeeDataAcess: EmployeeIDatacess
    {
        public static List<EmployeeModel> GetAllStoresFromDB()
        {
            List<EmployeeModel> models = new List<EmployeeModel>();
            using (SqlConnection connection = new SqlConnection(AppConnection.rSCConnectionString))

            using (SqlCommand cmd = new SqlCommand("Select EmployeeIdPk,EmpCode, EmployeeName, Role, DateOfJoining, DateOfLeaving,ContactNumber,Gender,Salary FROM Employees", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmployeeModel EmployeeObj = new EmployeeModel();
                        EmployeeObj.EmployeeIdPk = Convert.ToInt32(reader["EmployeeIdPk"]);
                        EmployeeObj.EmployeeCode = Convert.ToString(reader["EmpCode"]);
                        EmployeeObj.EmployeeName = Convert.ToString(reader["EmployeeName"]);
                        EmployeeObj.Role = Convert.ToString(reader["Role"]);
                        EmployeeObj.DateOfJoining = Convert.ToDateTime(reader["DateOfJoining"]);
                        EmployeeObj.DateOfLeaving = Convert.ToDateTime(reader["DateOfLeaving"]);
                        EmployeeObj.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                        EmployeeObj.Gender = Convert.ToString(reader["Gender"]);
                        EmployeeObj.Salary = Convert.ToDecimal(reader["Salary"]);
                        models.Add(EmployeeObj);
                    }
                }
                connection.Close();
            }

            return models;
        }
        public void PushEmployeeDataToDB(EmployeeModel employeeData)
        {
            using (SqlConnection conn = new SqlConnection(AppConnection.rSCConnectionString))
            {
                string query = "If Exists(select EmpCode from employees where EmpCode=@EmployeeCode)\r\nbegin\r\nupdate employees set role=@Role,ContactNumber=@ContactNumber,Salary=@Salary where empCode=@EmployeeCode\r\nend\r\nelse\r\nbegin\r\ninsert into Employees(EmpCode,EmployeeName,Role,DateOfJoining,DateOfLeaving,ContactNumber,Gender,Salary)\r\nvalues(@EmployeeCode,@EmployeeName,@Role,@DateOfJoining,@DateOfLeaving,@ContactNumber,@Gender,@Salary)\r\nend";
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@EmployeeCode", DbType.String).Value = employeeData.EmployeeCode;
                    command.Parameters.Add("EmployeeName", DbType.String).Value = employeeData.EmployeeName;
                    command.Parameters.Add("Role", DbType.String).Value = employeeData.Role;
                    command.Parameters.Add("DateOfJoining", DbType.String).Value = employeeData.DateOfJoining;
                    command.Parameters.Add("DateOIfLeaving", DbType.String).Value = employeeData.DateOfLeaving;
                    command.Parameters.Add("ContactNumber", DbType.String).Value = employeeData.ContactNumber;
                    command.Parameters.Add("Gender", DbType.String).Value = employeeData.Gender;
                    command.Parameters.Add("salary", DbType.String).Value = employeeData.Salary;

                }

            }



        }

    }
    
}
