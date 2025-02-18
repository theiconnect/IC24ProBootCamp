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
	}
}

	

