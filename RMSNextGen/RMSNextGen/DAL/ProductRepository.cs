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
		//public  List<ProductListDTO> GetProducts1()
		//{
		//	List<ProductListDTO> productListObj = new List<ProductListDTO>();
		//	using (SqlConnection connection = new SqlConnection(_connectionString))
		//	{
		//		 connection.Open();
		//		using (SqlCommand command = new SqlCommand())
		//		{
		//			command.CommandText = "select  ProductIDPk, ProductCode,ProductName,PricePerUnit from ProductMaster;";
					
		//			command.Connection = connection;
		//			try
		//			{
		//				using (SqlDataReader reader = command.ExecuteReader())
		//				{
		//					while (reader.Read())
		//					{

		//						ProductListDTO productListDTOObj = new ProductListDTO();
		//						productListDTOObj.ProductID = Convert.ToInt32(reader["ProductIDPk"]);
		//						productListDTOObj.ProductCode = Convert.ToString(reader["ProductCode"]);
		//						productListDTOObj.ProductName = Convert.ToString(reader["ProductName"]);
		//						productListDTOObj.PricePerUnit = Convert.ToString(reader["PricePerUnit"]);
		//						productListDTOObj.Quantity =0.0m;
		//						productListObj.Add(productListDTOObj);



							
		//					}

		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				throw ex;
		//			}
		//			finally
		//			{
		//				connection.Close();
		//			}
					


		//		}
		//	}
		//	return productListObj;
		//}
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

		public async Task<bool> GetProductCode()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (SqlCommand command = new SqlCommand()) 
				{
					command.Connection = connection;
					command.CommandText = "select max(productCode) from productMaster";
					try
					{
							string productCodeFromDB = (string)command.ExecuteScalar();

						productCodeFromDB = Convert.ToString("productCode");
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

		public List<ProductCategoryDTO> GetProductCategory()
		{
			List<ProductCategoryDTO> productCategoryList = new List<ProductCategoryDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select  ProductCategoryIDPk, ProductCategoryCode,ProductCategoryName from ProductCategory";

					command.Connection = connection;
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								ProductCategoryDTO ProductCategoryDTOObj = new ProductCategoryDTO();
								ProductCategoryDTOObj.ProductCategoryId = Convert.ToInt32(reader["ProductCategoryIDPk"]);
								ProductCategoryDTOObj.ProductCategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
								ProductCategoryDTOObj.ProductCategoryName = Convert.ToString(reader["ProductCategoryName"]);

								productCategoryList.Add(ProductCategoryDTOObj);




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
			return productCategoryList;
		}
		public List<ProductUTMDTO> GetUTM()
		{
			List<ProductUTMDTO> ProductUTMList = new List<ProductUTMDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select  UOMIdPk, UOMCode,UOM from UOM";

					command.Connection = connection;
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								ProductUTMDTO ProductUTMObj = new ProductUTMDTO();
								ProductUTMObj.UOMIdPk = Convert.ToInt32(reader["UOMIdPk"]);
								ProductUTMObj.UOMCode = Convert.ToString(reader["UOMCode"]);
								ProductUTMObj.UOMName = Convert.ToString(reader["UOM"]);

								ProductUTMList.Add(ProductUTMObj);




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
			return ProductUTMList;
		}

		public async Task<bool> GetProductBasedOnId(ProductEditDTO productEditObj)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select ProductIDPk, ProductCode,ProductName,PricePerUnit,ThresholdLimit from ProductMaster where ProductIDPk=@ProductIDPk ";
					command.Connection = conn;
					command.Parameters.AddWithValue("@productIdPk", productEditObj.ProductIdPk);
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								productEditObj.ProductIdPk = Convert.ToInt32(reader["ProductIDPk"]);
								productEditObj.ProductCode = Convert.ToString(reader["ProductCode"]);
								productEditObj.ProductName = Convert.ToString(reader["ProductName"]);
								productEditObj.PricePerUnit = Convert.ToDecimal(reader["pricePerUnit"]);
								productEditObj.ThresholdLimit = Convert.ToDecimal(reader["ThresholdLimit"]);


							}

						}
					}



					catch (Exception ex)
					{
						throw ex;
					}
					finally
					{
						conn.Close();
					}

				}
				return true;
			}

		}

		public async Task<bool> UpdateProducts(ProductEditDTO productEditObj)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "Update ProductMaster set ProductCode=@ProductCode,ProductName=@ProductName,PricePerUnit=@PricePerUnit,ThresholdLimit=@ThresholdLimit,ProductCategoryIdFk=@ProductCategoryIdFk where ProductIDPk=@ProductIDPk";
					command.Connection = conn;
					try
					{
						command.Parameters.AddWithValue("@ProductIDPk", productEditObj.ProductIdPk);
						command.Parameters.AddWithValue("@ProductCode", productEditObj.ProductCode);
						command.Parameters.AddWithValue("@ProductName", productEditObj.ProductName);

						command.Parameters.AddWithValue("@PricePerUnit", productEditObj.PricePerUnit);
						command.Parameters.AddWithValue("@ThresholdLimit", productEditObj.ThresholdLimit);
						command.Parameters.AddWithValue("@ProductCategoryIdFk", productEditObj.CategoryId);
						command.Parameters.AddWithValue("@UOMIdFk", productEditObj.UnitofMeasurementID);






						await command.ExecuteNonQueryAsync();
						return true;

					}
					catch (Exception ex)
					{
						return false;
					}
					finally
					{
						conn.Close();
					}


				}

			}
		}
	}

}


