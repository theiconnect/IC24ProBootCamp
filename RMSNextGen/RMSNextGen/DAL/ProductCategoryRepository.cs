using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RMSNextGen.Models;


namespace RMSNextGen.DAL
{
    public class ProductCategoryRepository
    {
        public readonly string _connectionString;
        public List<ProductCategoryListDTO> listDTOs { get; set; }

        public ProductCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
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
                string query = "select ProductCategoryIdPk,ProductCategoryCode,ProductCategoryName,Description from ProductCategory";

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
                                category.CategoryId = Convert.ToInt32(reader["ProductCategoryIdPk"]);
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
        public List<ProductCategoryListDTO> SearchProductCategory(SearchProductCategoryDTO searchObj)
        {
            List<ProductCategoryListDTO> ProductCategoryListObj = new List<ProductCategoryListDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "select ProductCategoryIDPk, ProductCategoryCode, ProductCategoryName,Description from ProductCategory where (@ProductCategoryCode IS NULL or ProductCategoryCode=@ProductCategoryCode) and (@ProductCategoryName IS NULL or ProductCategoryName=@ProductCategoryName)";

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
                                ProductCategoryObj.CategoryId = Convert.ToInt32(reader["ProductCategoryIDPk"]);
                                ProductCategoryObj.CategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
                                ProductCategoryObj.CategoryName = Convert.ToString(reader["ProductCategoryName"]);
                                ProductCategoryObj.Description = Convert.ToString(reader["Description"]);
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
        public async Task<bool> EditCategoryId(CategoryEditDTO CategoryEditObj)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "select ProductCategoryIDPk,ProductCategoryCode,ProductCategoryName,Description from ProductCategory where ProductCategoryIDPk=@ProductCategoryIDPk";
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@ProductCategoryIDPk", CategoryEditObj.ProductCategoryIdPk);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryEditObj.CategoryCode = Convert.ToString(reader["ProductCategoryCode"]);
                                CategoryEditObj.CategoryName = Convert.ToString(reader["ProductCategoryName"]);
                                CategoryEditObj.Description = Convert.ToString(reader["Description"]);
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
        public async Task<bool> UpdateEditCategory(CategoryEditDTO CategoryEditObj)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                
                
                
                    string Query = "UPDATE ProductCategory SET ProductCategoryCode = @ProductCategoryCode,    ProductCategoryName = @ProductCategoryName,    Description = @Description WHERE ProductCategoryIdPk = @ProductCategoryIdPk";
                await con.OpenAsync();
                using (SqlCommand command = new SqlCommand(Query,con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ProductCategoryIDPk", CategoryEditObj.ProductCategoryIdPk);
                        command.Parameters.AddWithValue("@ProductCategoryCode", CategoryEditObj.CategoryCode);
                        command.Parameters.AddWithValue("@ProductCategoryName", CategoryEditObj.CategoryName);
                        command.Parameters.AddWithValue("@Description", CategoryEditObj.Description);
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {

                        con.Close();
                    }
                }
                return true;
            }
        }
    }
}
 