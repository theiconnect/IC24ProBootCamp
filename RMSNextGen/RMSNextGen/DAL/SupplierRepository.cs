using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

		public async Task<bool> AddSupplier(SupplierDTO DTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();
					string query = "INSERT INTO Supplier (SupplierCode,SupplierName,CompanyName, ContactNumber1,ContactNumber2,Email, [Address], GSTNumber,CreatedBy,CreatedOn)" +
						"VALUES (@SupplierCode,@SupplierName,@CompanyName, @ContactNumber1,@ContactNumber2,@Email, @Address,@GSTNumber,@CreatedBy,@CreatedOn)";
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
		public  List<SupplierListDTO> GetSupplierList()
		{
			List<SupplierListDTO> SupplierListDTO = new List<SupplierListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "select SupplierIdPk,SupplierName,CompanyName,Address from Supplier";
				using (SqlCommand command = new SqlCommand(query,connection))
				{
					//command.Connection = connection;
					//command.CommandText = query;
					try
					{
						
						
						
						//command.CommandType = CommandType.StoredProcedure;
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								SupplierListDTO supplierListDTO = new SupplierListDTO();
								supplierListDTO.SupplierIdPk = Convert.ToInt32(reader["SupplierIdPk"]);
								supplierListDTO.SupplierName = Convert.ToString(reader["SupplierName"]);
								supplierListDTO.CompanyName = Convert.ToString(reader["CompanyName"]);
								supplierListDTO.Address = Convert.ToString(reader["Address"]);
								//supplierListDTO.City = Convert.ToString(reader["City"]);
								//supplierListDTO.State = Convert.ToString(reader["State"]);
								//model.ContactNumber = Convert.ToString(reader["StoreContactNumber"]);
								SupplierListDTO.Add(supplierListDTO);
							}
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error:" + ex.Message);
						
					}
					finally
					{
						connection.Close();
						//command.Dispose();

					}
				}

			}
			return SupplierListDTO;
		}


	}

}
	

