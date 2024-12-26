using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathAndDataBaseConfig;
using IDataAccess;

namespace DataAccess
{
    public class EmployeeDA:IEmployeeDA
    {
        public int storeIdFk { get; set; }
        public bool SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject)
        {
            using (SqlConnection con = new SqlConnection(BaseProcessor.rscConnectedString))
            {

                //EmployeeCode | StoreCode | EmployeeName | Role | DateOfJoining | DateOfLeaving | ContactNumber | Gender | Salary | StoreId
                //  string query = "IF EXISTS (SELECT employeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode)\r\nBEGIN\r\n    UPDATE Employee \r\n    SET \r\n        Role = @Role,\r\n        ContactNumber = @ContactNumber,\r\n        Salary = @Salary\r\n    WHERE \r\n        EmployeeCode = @EmployeeCode;\r\nEND\r\nELSE\r\nBEGIN\r\n    INSERT INTO Employee (EmployeeCode, EmployeeName, Role, DateOfJoining, DateOfLeaving, ContactNumber, Gender, Salary, StoreIdFk)\r\n    VALUES (@EmployeeCode, @EmployeeName, @Role, @DateOfJoining, @DateOfLeaving, @ContactNumber, @Gender, @Salary, \r\n        (SELECT StoreIdPk FROM stores WHERE StoreCode = @StoreCode));\r\nEND\r\n";
                try
                {
                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmployee", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var employee in fileEmployeeDTOObject)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@EmployeeCode", DbType.String).Value = employee.EmployeeCode;
                            cmd.Parameters.Add("@employeeName", DbType.String).Value = employee.EmployeeName;
                            cmd.Parameters.Add("@Role", DbType.String).Value = employee.Role;
                            cmd.Parameters.Add("@DateOfJoining", DbType.DateTime).Value = employee.DateOfJoining;
                            cmd.Parameters.Add("@DateOfLeaving", DbType.DateTime).Value = employee.DateOfLeaving;
                            cmd.Parameters.Add("@ContactNumber", DbType.String).Value = employee.ContactNumber;
                            cmd.Parameters.Add("@Gender", DbType.String).Value = employee.Gender;
                            cmd.Parameters.Add("@Salary", DbType.Decimal).Value = employee.Salary;
                            cmd.Parameters.Add("@StoreCode", DbType.String).Value = employee.StoreCode;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();

                    }
                }
                
            }
            return true;
        }
    }
}
