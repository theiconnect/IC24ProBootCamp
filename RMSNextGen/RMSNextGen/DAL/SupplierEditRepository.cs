using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
    public class SupplierEditRepository
    {
		private readonly string _connectionString;

		public SupplierEditRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<bool> EditSupplierDetails(SupplierEditDTO DTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();
					//string query = "INSERT INTO Supplier (SupplierCode,SupplierName,CompanyName, ContactNumber1,ContactNumber2,Email, [Address], GSTNumber,CreatedBy,CreatedOn)" +
					//	"VALUES (@SupplierCode,@SupplierName,@CompanyName, @ContactNumber1,@ContactNumber2,@Email, @Address,@GSTNumber,@CreatedBy,@CreatedOn)";


					string query = "UPDATE Supplier " +
				   "SET SupplierName = @SupplierName, " +
				   "    CompanyName = @CompanyName, " +
				   "    ContactNumber1 = @ContactNumber1, " +
				   "    ContactNumber2 = @ContactNumber2, " +
				   "    Email = @Email, " +
				   "    [Address] = @Address, " +
				   "    GSTNumber = @GSTNumber, " +
				   "    CreatedBy = @CreatedBy, " +
				   "    CreatedOn = @CreatedOn " +
				   "WHERE SupplierCode = @SupplierCode";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@SupplierCode", DTO.SupplierCode);
						command.Parameters.AddWithValue("@SupplierName", DTO.SupplierName);
						command.Parameters.AddWithValue("@CompanyName", DTO.CompanyName);
						command.Parameters.AddWithValue("@ContactNumber1", DTO.ContactNumber1);
						command.Parameters.AddWithValue("@ContactNumber2", DTO.ContactNumber2);
						command.Parameters.AddWithValue("@Email", DTO.Email);
						command.Parameters.AddWithValue("@Address", DTO.Address);
						command.Parameters.AddWithValue("@GSTNumber ", DTO.GSTNumber);
						command.Parameters.AddWithValue("@CreatedBy", DTO.CreatedBy);
						command.Parameters.AddWithValue("@CreatedOn", DateTime.Today);
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

    

