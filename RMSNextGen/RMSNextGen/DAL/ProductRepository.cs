using Microsoft.Data.SqlClient;
using RMSNextGen.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
	public class ProductRepository
	{
		public readonly string _connectionString;

		public ProductRepository(string connectionString) 
		{
			_connectionString = connectionString;
		}
		public async Task<bool> SaveProduct(ProductDTO ProductObj)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (SqlCommand command=new SqlCommand())
				{
					command.CommandText = "Insert Into ProductMaster(ProductCode,ProductName,PricePerUnit,ThresholdLimit,CreatedBy,CreatedOn)" +
						"values(@ProductCode,@ProductName,@PricePerUnit,@ThresholdLimit,@CreatedBy, GetDate())";
					command.Connection = connection;
					try
					{
						command.Parameters.AddWithValue("@ProductCode", ProductObj.ProductCode);
						command.Parameters.AddWithValue("@ProductName", ProductObj.ProductName);
						
						command.Parameters.AddWithValue("@PricePerUnit", ProductObj.PricePerUnit);
						command.Parameters.AddWithValue("@ThresholdLimit", ProductObj.ThresholdLimit);
						
						command.Parameters.AddWithValue("@CreatedBy", ProductObj.CreatedBy);
						

						await command.ExecuteNonQueryAsync();
						return true;

					}
					catch (Exception ex) 
					{ 
						return false;
					}
					finally
					{
						connection.Close();
					}

				}

			}
			
		}
	}
}
