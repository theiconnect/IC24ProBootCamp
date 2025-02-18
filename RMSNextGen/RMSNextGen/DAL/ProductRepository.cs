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
				using (SqlCommand command = new SqlCommand())
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
		public List<ProductListDTO> GetProducts1()
		{
			List<ProductListDTO> productListObj = new List<ProductListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select  ProductIDPk, ProductCode,ProductName,PricePerUnit from ProductMaster;";

					command.Connection = connection;
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								ProductListDTO productListDTOObj = new ProductListDTO();
								productListDTOObj.ProductID = Convert.ToInt32(reader["ProductIDPk"]);
								productListDTOObj.ProductCode = Convert.ToString(reader["ProductCode"]);
								productListDTOObj.ProductName = Convert.ToString(reader["ProductName"]);
								productListDTOObj.PricePerUnit = Convert.ToString(reader["PricePerUnit"]);
								productListDTOObj.Quantity = 0.0m;
								productListObj.Add(productListDTOObj);




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
			return productListObj;
		}
		public List<ProductListDTO> GetProducts(ProductSearchDTO searchObj)
		{
			List<ProductListDTO> productListObj = new List<ProductListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select  ProductIDPk, ProductCode,ProductName,PricePerUnit from ProductMaster where (@ProductCode IS NULL or ProductCode=@ProductCode) and (@ProductName IS NULL or ProductName=@ProductName);";
					
					command.Parameters.AddWithValue("@ProductCode", searchObj.ProductCode==null? DBNull.Value : searchObj.ProductCode);
					command.Parameters.AddWithValue("@ProductName", searchObj.ProductName==null? DBNull.Value : searchObj.ProductName);
					command.Connection = connection;
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								ProductListDTO productListDTOObj = new ProductListDTO();
								productListDTOObj.ProductID = Convert.ToInt32(reader["ProductIDPk"]);
								productListDTOObj.ProductCode = Convert.ToString(reader["ProductCode"]);
								productListDTOObj.ProductName = Convert.ToString(reader["ProductName"]);
								productListDTOObj.PricePerUnit = Convert.ToString(reader["PricePerUnit"]);
								productListDTOObj.Quantity = 0.0m;
								productListObj.Add(productListDTOObj);




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
			return productListObj;
		}
	}
}


