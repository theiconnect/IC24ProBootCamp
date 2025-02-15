using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
	public class SupplierRepository
	{
		private readonly string _connectionString;

		public SupplierRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
		public async Task<bool> SaveSupplier(SupplierDTO DTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();
					string query = "INSERT INTO Supplier (FirstName,  LastName,CompanyName, MobileNumber, Address, City, State,FaxNumber,GstNo) " +
								   "VALUES (@FirstName, @LastName,@CompanyName, @MobileNumber, @Address, @City, @State,@FaxNumber,@GstNo)";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@FirstName", DTO.FirstName);
						command.Parameters.AddWithValue("@LastName", DTO.LastName);
						command.Parameters.AddWithValue("@CompanyName", DTO.CompanyName);
						command.Parameters.AddWithValue(" MobileNumber", DTO.MobileNumber);
						command.Parameters.AddWithValue("@Address", DTO.Address);
						command.Parameters.AddWithValue("@City", DTO.City);
						command.Parameters.AddWithValue("@State ", DTO.State);
						command.Parameters.AddWithValue("@FaxNumber ", DTO.FaxNumber);
						command.Parameters.AddWithValue("@GstNo ", DTO.GstNo);
						await command.ExecuteNonQueryAsync();
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
