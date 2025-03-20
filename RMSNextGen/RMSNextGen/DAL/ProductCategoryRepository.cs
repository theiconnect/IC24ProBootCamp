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
		public List<ProductCategoryListDTO> ProductCategoryList(CategorySearchDTO searchobj)
		{ 
			List<ProductCategoryListDTO> productCategoryList = new List<ProductCategoryListDTO>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open(); 

				string query = "SELECT ProductCategoryIdPk, ProductCategoryCode, ProductCategoryName, Description FROM ProductCategory where(@ProductCategoryCode IS NULL or ProductCategoryCode = @ProductCategoryCode) and(@ProductCategoryName IS NULL or ProductCategoryName = @ProductCategoryName);";
			
			  
				using (SqlCommand command = new SqlCommand(query, connection)) 
				{
					command.Parameters.AddWithValue("@ProductCategoryCode", searchobj.CategoryCode == null ? DBNull.Value :  searchobj.CategoryCode);
					command.Parameters.AddWithValue("@ProductCategoryName", searchobj.CategoryName == null ? DBNull.Value :
						searchobj.CategoryName);
					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{

							 ProductCategoryListDTO product = new ProductCategoryListDTO();
								product.CategoryIdPK = Convert.ToInt32(reader["ProductCategoryIdPk"]);
								product.CategoryCode = Convert.ToString (reader                                              ["ProductCategoryCode"]);
								product.CategoryName = Convert.ToString (reader                                              ["ProductCategoryName"]);
								product.Description = Convert.ToString  (reader                                                ["Description"]);
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

		public async Task<bool> EditcategoryId(EditCategoryDTO editproductcategory)
		{
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{  
				 await connection.OpenAsync();
				string query = "Select ProductCategoryCode,ProductCategoryName,Description from ProductCategory where ProductCategoryIdPk =@ProductCategoryIdPk";
				using (SqlCommand command = new SqlCommand(query,connection))
				{
					command.Parameters.AddWithValue("@ProductCategoryIdPk",
						editproductcategory.ProductCategoryIdPk);
					
					try
					{
						using (SqlDataReader reader =  command.ExecuteReader())
						{
							while (reader.Read())
							{


								editproductcategory.CategoryCode = Convert.ToString(reader            ["ProductCategoryCode"]);
								     editproductcategory.CategoryName = Convert.ToString             (reader["ProductCategoryName"]);
								editproductcategory.Description = Convert.ToString(reader             ["Description"]);

							}
								
						}
					}
					catch (Exception ex)
					{

						Console.WriteLine("Error: " + ex.Message);
					}
				}
			}
			return true;
		}
		public async Task<bool> Updatecategory(EditCategoryDTO editproductcategory)

		{
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = "update ProductCategory set ProductCategoryCode=@ProductCategoryCode,ProductCategoryName=@ProductCategoryName,Description=@Description where ProductCategoryIdPk =@ProductCategoryIdPk";
				using(SqlCommand  command = new SqlCommand(query,connection))
				{
					
					try
					{
						command.Parameters.AddWithValue("@ProductCategoryIdPk",
						editproductcategory.ProductCategoryIdPk);
						command.Parameters.AddWithValue("@ProductCategoryName", editproductcategory.CategoryName);
						command.Parameters.AddWithValue("@ProductCategoryCode", editproductcategory.CategoryCode);
						command.Parameters.AddWithValue("@Description", editproductcategory.Description);

						await command.ExecuteNonQueryAsync();



					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}

			}
			return true;
		}
		
	}


	
}
