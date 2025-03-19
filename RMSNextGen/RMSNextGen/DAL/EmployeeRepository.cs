using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
                    
					using (SqlCommand cmd = new SqlCommand())

					{
                        cmd.CommandText = "usp_UpsertEmployee";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@EmployeeFirstName", EmpDTO.EmployeeFirstName);
						cmd.Parameters.AddWithValue("@EmployeeLastName",(object) EmpDTO.EmployeeLastName  ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Email", EmpDTO.Email);
						cmd.Parameters.AddWithValue("@MobileNumber", EmpDTO.MobileNumber);
						cmd.Parameters.AddWithValue("@Department",(object) EmpDTO.Department ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Designation", (object)EmpDTO.Designation ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PersonalEmail", (object)EmpDTO.PersonalEmail ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Gender", EmpDTO.Gender);
						cmd.Parameters.AddWithValue("@Salary", (object)EmpDTO.SalaryCTC ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PermanentAddressLine1", (object)EmpDTO.PermanentAddressline1 ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PermanentAddressLine2", (object)EmpDTO.PermanentAddressline2 ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PermanentCity", (object)EmpDTO.PermanentCity ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PermanentState", (object)EmpDTO.PermanentState ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PermanentPincode", (object)EmpDTO.PermanentPincode ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@AddressLine1", (object)EmpDTO.CurrentAddressline1 ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@AddressLine2", (object)EmpDTO.CurrentAddressline2 ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@City", (object)EmpDTO.CurrentCity ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@State", (object)EmpDTO.CurrentState ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Pincode", (object)EmpDTO.CurrentPincode ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@CreatedBy", EmpDTO.CreatedBy);
						cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Today);
						cmd.Parameters.AddWithValue("@LastUpdatedBy", (object)EmpDTO.LastUpdatedBy ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@LastUpdatedOn", DateTime.Today);

						await connection.OpenAsync();
						await cmd.ExecuteNonQueryAsync();
						 return true;	
					}
				}

			}
			catch (Exception ex)
			{
				return false;
			}

		}
		public List<EmployeeListDTO> GetEmployees(EmployeeSearchDTO searchObj)
		{
			var employees = new List<EmployeeListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "usp_SearchEmployee";
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Connection = connection;
					if (searchObj != null)
					{
						if (!string.IsNullOrWhiteSpace(searchObj.EmployeeCode))
							command.Parameters.AddWithValue("@EmployeeCode", searchObj.EmployeeCode);
						if (!string.IsNullOrWhiteSpace(searchObj.EmployeeName))
							command.Parameters.AddWithValue("@EmployeeName", searchObj.EmployeeName);
						if (!string.IsNullOrWhiteSpace(searchObj.MobileNumber))
							command.Parameters.AddWithValue("@Mobile", searchObj.MobileNumber);
						command.Parameters.AddWithValue("@EmployeeCode", searchObj.DepartmentId);
					}

					try
					{
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var emp = new EmployeeListDTO();
								emp.EmployeeID = Convert.ToInt32(reader["EmployeeIdPk"]);
								emp.EmployeeCode = Convert.ToString(reader["EmployeeCode"]);
								emp.EmployeeName = Convert.ToString(reader["EmployeeName"]);
								emp.Department = Convert.ToString(reader["DepartmentName"]);
								emp.Designation = Convert.ToString(reader["Designation"]);
								emp.MobileNumber = Convert.ToString(reader["MobileNumber"]);
								emp.StoreCode = Convert.ToString(reader["StoreCode"]);
								employees.Add(emp);
							}
						}
					}
					catch (Exception ex)
					{
						throw;
					}
					finally
					{
						if (connection.State == System.Data.ConnectionState.Open)
							connection.Close();
					}
				}
			}
			return employees;
		}

		public async Task<bool> EditEmployee(EmployeeEditDTO employeeEditDTO)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "select EmployeeidPk,EmployeeCode, EmployeeFirstName, EmployeeLastName, Email, MobileNumber,  Department, " +
					"Designation, PersonalEmail, Gender, SalaryCTC, PermanentAddressLine1," +
					" PermanentAddressLine2, PermanentCity, PermanentState, PermanentPincode, " +
					"CurrentAddressLine1, CurrentAddressLine2, CurrentCity, CurrentState, CurrentPincode from Employee where EmployeeidPk=@EmployeeidPK";

				using (SqlCommand cmd = new SqlCommand(query, connection))
				{
					await connection.OpenAsync();

					cmd.Parameters.AddWithValue("@EmployeeidPk", employeeEditDTO.EmployeeidPK);
					try
					{
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								EmployeeEditDTO _employeeEditDTO = new EmployeeEditDTO();
								_employeeEditDTO.EmployeeidPK = Convert.ToInt32(reader["EmployeeidPk"]);
								_employeeEditDTO.EmployeeFirstName = Convert.ToString(reader["EmployeeFirstName"]);
								_employeeEditDTO.EmployeeLastName = Convert.ToString(reader["EmployeeLastName"]);
								_employeeEditDTO.Email = Convert.ToString(reader["Email"]);
								_employeeEditDTO.MobileNumber = Convert.ToString(reader["MobileNumber"]);
								_employeeEditDTO.Department = Convert.ToString(reader["Department"]);
								_employeeEditDTO.Designation = Convert.ToString(reader["Designation"]);
								_employeeEditDTO.PersonalEmail = Convert.ToString(reader["PersonalEmail"]);
								_employeeEditDTO.Gender = Convert.ToString(reader["Gender"]);
								_employeeEditDTO.Salary = Convert.ToInt32(reader["SalaryCTC"]);
								_employeeEditDTO.PermanentAddressLine1 = Convert.ToString(reader["PermanentAddressLine1"]);
								_employeeEditDTO.PermanentAddressline2 = Convert.ToString(reader["PermanentAddressLine2"]);
								_employeeEditDTO.PermanentCity = Convert.ToString(reader["PermanentCity"]);
								_employeeEditDTO.PermanentState = Convert.ToString(reader["PermanentState"]);
								_employeeEditDTO.PermanentPincode = Convert.ToInt32(reader["PermanentPincode"]);
								_employeeEditDTO.CurrentAddressline1 = Convert.ToString(reader["CurrentAddressline1"]);
								_employeeEditDTO.CurrentAddressline2 = Convert.ToString(reader["CurrentAddressline2"]);
								_employeeEditDTO.CurrentCity = Convert.ToString(reader["CurrentCity"]);
								_employeeEditDTO.CurrentState = Convert.ToString(reader["CurrentState"]);
								_employeeEditDTO.CurrentPincode = Convert.ToInt32(reader["CurrentPincode"]);
								




							}
						}
					}

					catch (Exception ex)
					{
						throw ex;
					}
					finally
					{
						connection.Close();
					}
				}
				return true;

			}	

		}
			
	}

}



	

