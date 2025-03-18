using System;
using System.Collections.Generic;
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
					string query = "INSERT INTO Employee (EmployeeCode, EmployeeFirstName, EmployeeLastName, Email, MobileNumber,  Department, Designation, PersonalEmail, Gender, SalaryCTC, PermanentAddressLine1, PermanentAddressLine2, PermanentCity, PermanentState, PermanentPincode, CurrentAddressLine1, CurrentAddressLine2, CurrentCity, CurrentState, CurrentPincode, CreatedBy, CreatedOn, LastUpdatedBy, LastUpdatedOn) " +
								   "VALUES (@EmployeeCode, @EmployeeFirstName, @EmployeeLastName,@Email, @MobileNumber, @Department, @Designation, @PersonalEmail, @Gender, @Salary, @PermanentAddressLine1, @PermanentAddressLine2, @PermanentCity, @PermanentState, @PermanentPincode, @AddressLine1, @AddressLine2, @City, @State, @Pincode, @CreatedBy, @CreatedOn, @LastUpdatedBy, @LastUpdatedOn);";

				
					using (SqlCommand cmd = new SqlCommand(query, connection))
					{
						await connection.OpenAsync();
						cmd.Parameters.AddWithValue("@EmployeeCode", EmpDTO.EmployeeCode);
						cmd.Parameters.AddWithValue("@EmployeeFirstName", EmpDTO.EmployeeFirstName);
						cmd.Parameters.AddWithValue("@EmployeeLastName", EmpDTO.EmployeeLastName);
						cmd.Parameters.AddWithValue("@Email", EmpDTO.Email);
						cmd.Parameters.AddWithValue("@MobileNumber", (object)EmpDTO.MobileNumber ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Department", EmpDTO.Department);
						cmd.Parameters.AddWithValue("@Designation", EmpDTO.Designation);
						cmd.Parameters.AddWithValue("@PersonalEmail", (object)EmpDTO.PersonalEmail ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Gender", EmpDTO.Gender);
						cmd.Parameters.AddWithValue("@Salary", EmpDTO.SalaryCTC);
						cmd.Parameters.AddWithValue("@PermanentAddressLine1", EmpDTO.PermanentAddressline1);
						cmd.Parameters.AddWithValue("@PermanentAddressLine2", EmpDTO.PermanentAddressline2);
						cmd.Parameters.AddWithValue("@PermanentCity", EmpDTO.PermanentCity);
						cmd.Parameters.AddWithValue("@PermanentState", EmpDTO.PermanentState);
						cmd.Parameters.AddWithValue("@PermanentPincode", EmpDTO.PermanentPincode);
						cmd.Parameters.AddWithValue("@AddressLine1", EmpDTO.CurrentAddressline1);
						cmd.Parameters.AddWithValue("@AddressLine2", (object)EmpDTO.CurrentAddressline2 ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@City", EmpDTO.CurrentCity);
						cmd.Parameters.AddWithValue("@State", EmpDTO.CurrentState);
						cmd.Parameters.AddWithValue("@Pincode", EmpDTO.CurrentPincode);
						cmd.Parameters.AddWithValue("@CreatedBy", (object)EmpDTO.CreatedBy ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Today);
						cmd.Parameters.AddWithValue("@LastUpdatedBy", (object)EmpDTO.LastUpdatedBy ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@LastUpdatedOn", DateTime.Today);

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
		public List<EmployeeListDTO> GetEmployee(EmployeeSearchDTO searchObj)
		{
			List<EmployeeListDTO> ElDTO = new List<EmployeeListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{ 
				connection.Open();
				using (SqlCommand command = new SqlCommand()) 
				{
					command.CommandText = "select  EmployeeIdPk,Employeecode,EmployeeFirstName,EmployeeLastName,Gender,Designation,MobileNumber,StoreIdFk from Employee where(@EmployeeCode IS NULL or EmployeeCode = @EmployeeCode) " +
						"and (@MobileNumber IS NULL or MobileNumber=@MobileNumber)" +/* "and (@StoreCode IS NULL or StoreCode = @StoreCode)" +*/ "and (@Designation IS NULL or Designation = @Designation)";

					command.Connection = connection;
					command.Parameters.AddWithValue("@EmployeeCode", searchObj.EmployeeCode == null ? DBNull.Value : searchObj.EmployeeCode);
					command.Parameters.AddWithValue("@MobileNumber", searchObj.MobileNumber == null ? DBNull.Value : searchObj.MobileNumber);
					//command.Parameters.AddWithValue("@StoreCode", searchObj.StoreCode == null ? DBNull.Value : searchObj.StoreCode);
					command.Parameters.AddWithValue("@Designation", searchObj.Designation == null ? DBNull.Value : searchObj.Designation);


					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								EmployeeListDTO EmpListDTO = new EmployeeListDTO();
								EmpListDTO.EmployeeID = Convert.ToInt32(reader["EmployeeIdPk"]);
								EmpListDTO.EmployeeCode = Convert.ToString(reader["EmployeeCode"]);
								EmpListDTO.EmployeeName = Convert.ToString(reader["EmployeeFirstName"]) + " " + Convert.ToString(reader["EmployeeLastName"]);
								EmpListDTO.Gender = Convert.ToString(reader["Gender"]);
								EmpListDTO.Designation = Convert.ToString(reader["Designation"]);
								EmpListDTO.MobileNumber = Convert.ToString(reader["MobileNumber"]);
								EmpListDTO.StoreIdFk = Convert.ToString(reader["StoreIdFk"]);
								ElDTO.Add(EmpListDTO);



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
			}
			return ElDTO;
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



	

