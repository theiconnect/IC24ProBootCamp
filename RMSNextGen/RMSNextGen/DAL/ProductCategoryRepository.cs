using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using RMSNextGen.Models;


namespace RMSNextGen.DAL
{
    public class ProductCategoryRepository
    {
        public readonly string _connectionString;
        public ProductCategoryRepository(string connectionString)
        {
            _connectionString=connectionString;
        }

        public async Task<bool> AddCategory(ProductCategoryDTO obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO ProductCategory(ProductCategoryCode, ProductCategoryName, Description, CreatedBy) " +
                                   "VALUES (@ProductCategoryCode, @ProductCategoryName, @Description, @CreatedBy)";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductCategoryCode", obj.CategoryCode);
                        command.Parameters.AddWithValue("@ProductCategoryName", obj.CategoryName);
                        command.Parameters.AddWithValue("@Description", obj.Description);
                        command.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy); 
                        command.Parameters.AddWithValue("@CreatedOn",DateTime.Today);
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
		public List<ProductCategoryListDTO> ProductCategoryList()
		{
			List<ProductCategoryListDTO> productCategoryList = new List<ProductCategoryListDTO>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open(); 

				string query = "SELECT ProductCategoryIdPk, ProductCategoryCode, ProductCategoryName, Description FROM ProductCategory";

				using (SqlCommand command = new SqlCommand(query, connection)) 
				{
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								ProductCategoryListDTO product = new ProductCategoryListDTO();
								product.CategoryIdPK = Convert.ToString(reader["ProductCategoryIdPk"]);
								product.CategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
								product.CategoryName = Convert.ToString(reader["ProductCategoryName"]);
								product.Description = Convert.ToString(reader["Description"]);
								productCategoryList.Add(product);
								


							}
						}
					} 
					catch (Exception ex)
					{
						
						Console.WriteLine("Error: " + ex.Message);
					}
				}
			}

			return productCategoryList;
		}
		public List<ProductCategoryListDTO> SearchProductCategory(CategorySearchDTO searchObj)
		{
			List<ProductCategoryListDTO> ProductCategoryListObj = new List<ProductCategoryListDTO>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "select  ProductCategoryCode, ProductCategoryName from ProductCategory where (@ProductCategoryCode IS NULL or ProductCategoryCode=@ProductCategoryCode) and (@ProductCategoryName IS NULL or ProductCategoryName=@ProductCategoryName);";

					command.Parameters.AddWithValue("@ProductCategoryCode", searchObj.CategoryCode == null ? DBNull.Value : searchObj.CategoryCode);
					command.Parameters.AddWithValue("@ProductCategoryName", searchObj.CategoryName == null ? DBNull.Value : searchObj.CategoryName);
					command.Connection = connection;
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

								ProductCategoryListDTO ProductCategoryObj = new ProductCategoryListDTO();
								ProductCategoryObj.CategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
								ProductCategoryObj.CategoryName = Convert.ToString(reader["ProductCategoryName"]);
								ProductCategoryListObj.Add(ProductCategoryObj);

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
			return ProductCategoryListObj;

		}


	}
}
