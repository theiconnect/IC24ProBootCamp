using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RMSNextGen.Models;


namespace RMSNextGen.DAL
{
    public class ProductCategoryRepository
    {
        public readonly string _connectionString;
         public List<ProductCategoryListDTO> listDTOs {  get; set; }    

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
                    string query = "INSERT INTO ProductCategory(ProductCategoryCode, ProductCategoryName, Description, CreatedBy, CreatedOn) " +
                                   "VALUES (@ProductCategoryCode, @ProductCategoryName, @Description, @CreatedBy, 10/02/2025)";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductCategoryCode", obj.CategoryCode);
                        command.Parameters.AddWithValue("@ProductCategoryName", obj.CategoryName);
                        command.Parameters.AddWithValue("@Description", obj.Description);
                        command.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy); 
                        command.Parameters.AddWithValue("@CreatedOn", 10 / 02 / 2025); 
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
        public List<ProductCategoryListDTO> GetProductCategoryList()
        {
            listDTOs = new List<ProductCategoryListDTO>();  
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "select ProductCategoryIdPk,ProductCategoryCode,ProductCategoryName,[Description] from ProductCategory";
                     
                        connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            while (reader.Read()) 
                            {
                                ProductCategoryListDTO category = new ProductCategoryListDTO();
                                category.CategoryIdPK = Convert.ToInt32(reader["ProductCategoryIdPk"]);
                                category.CategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
                                category.CategoryName = Convert.ToString(reader["ProductCategoryName"]); 
                                category.Description = Convert.ToString(reader["Description"]);
                                listDTOs.Add(category);
                            }
                        }
                    }
                    catch (Exception ex) 
                        {
                            throw ex;
                        }
                    }
                }
                return listDTOs;    
        }
    }
}
