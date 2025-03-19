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
		public  List<SupplierListDTO> GetSupplierList(SearchSupplierDTO searchDTO)
		{
			List<SupplierListDTO> SupplierListDTO = new List<SupplierListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "select SupplierIdpk,SupplierName,CompanyName,Address from Supplier" +
					" where (@SupplierName is null or SupplierName=@SupplierName)and (@CompanyName is null or CompanyName=@CompanyName) and (@Address is null or Address=@Address)";

				using (SqlCommand command = new SqlCommand(query,connection))
				{
					command.Parameters.AddWithValue("@SupplierName", searchDTO.SupplierName == null ? DBNull.Value : searchDTO.SupplierName);
					command.Parameters.AddWithValue("@CompanyName", searchDTO.CompanyName == null ? DBNull.Value : searchDTO.CompanyName);
					command.Parameters.AddWithValue("@Address", searchDTO.Address == null ? DBNull.Value : searchDTO.Address);

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
		public async Task<bool> EditSupplierDetails(SupplierEditDTO DTO)
		{

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select SupplierIDpk,SupplierName,CompanyName,Address from Supplier where SupplierIDpk=@SupplierIDpk";
					command.Connection = connection;

					command.Parameters.AddWithValue("@SupplierIDpk", DTO.SupplierIdPk);


					try
					{
						//command.CommandType = CommandType.StoredProcedure;
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								DTO.SupplierName = Convert.ToString(reader["SupplierName"]);
								DTO.CompanyName = Convert.ToString(reader["CompanyName"]);
								DTO.Address = Convert.ToString(reader["Address"]);
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
			return true;
		}
		public async Task<bool> UpdateSupplierDetails(SupplierEditDTO DTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();
					string query = "update supplier set SupplierName=@SupplierName,CompanyName=@CompanyName,Address=@Address where SupplierIdpk=@SupplierIdpk";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@SupplierIdpk", DTO.SupplierIdPk);
						command.Parameters.AddWithValue("@SupplierName", DTO.SupplierName);
						command.Parameters.AddWithValue("@CompanyName", DTO.CompanyName);
						command.Parameters.AddWithValue("@Address", DTO.Address);

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


	

