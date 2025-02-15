using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RMSNextGen.Models;

namespace RMSNextGen.DAL
{
	public class EmployeeRepository
	{
		private readonly string _connectionString;
		public EmployeeRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
		public async Task<bool> SaveEmployee(EmployeeDTO EmpDTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{

					string query = "INSERT INTO EmployeeList(EmployeeCode, EmployeeName, StoreCode, Role, Gender, Salary, ContactNumber,Addressline1,Addressline2,City,State,Pincode) " +
								   "VALUES (@EmployeeCode, @EmployeeName, @StoreCode, @Role, @Gender, @Salary, @ContactNumber ,@Addressline1,@Addressline2,@City,@State,@Pincode)";
					connection.Open();
					using (SqlCommand cmd = new SqlCommand(query, connection))
					{
						cmd.Parameters.AddWithValue("@EmployeeCode", EmpDTO.EmployeeCode);
						cmd.Parameters.AddWithValue("@EmployeeName", EmpDTO.EmployeeName);
						cmd.Parameters.AddWithValue("@StoreCode", EmpDTO.StoreCode);
						cmd.Parameters.AddWithValue("@Role", EmpDTO.Role);
						cmd.Parameters.AddWithValue("@Gender", EmpDTO.Gender);
						cmd.Parameters.AddWithValue("@Salary", EmpDTO.Salary);
						cmd.Parameters.AddWithValue("@ContactNumber", EmpDTO.ContactNumber);
						cmd.Parameters.AddWithValue("@Addressline1", EmpDTO.Addressline1);
						cmd.Parameters.AddWithValue("@Addressline2", EmpDTO.Addressline2);
						cmd.Parameters.AddWithValue("@City", EmpDTO.City);
						cmd.Parameters.AddWithValue("@State", EmpDTO.State);
						cmd.Parameters.AddWithValue("@Pincode", EmpDTO.Pincode);

						cmd.ExecuteNonQuery();
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}

		}
	}
}

	

