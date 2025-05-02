using Microsoft.Data.SqlClient;
using RMSNextGen.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSAppUsingWebAPI.DAL
{
	public class StoreApiRepository
	{
		public string _connectionString;
		

		public StoreApiRepository(string connectionString)
		{
			_connectionString = connectionString;
			


		}

		public List<StoreListDTO> GetStores(SearchStoresDTO searchStores)
		{
			var storeList = new List<StoreListDTO>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				string query = @"SELECT StoreIdPk, StoreCode, Location, City, State FROM Store where (@StoreCode IS NULL OR StoreCode = @StoreCode)
              AND (@Location IS NULL OR Location = @Location)
              AND (@City IS NULL OR City = @City)
              AND (@State IS NULL OR State = @State)";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
                    command.Parameters.AddWithValue("@StoreCode", string.IsNullOrEmpty(searchStores.StoreCode) ? (object)DBNull.Value : searchStores.StoreCode);
                    command.Parameters.AddWithValue("@Location", string.IsNullOrEmpty(searchStores.Location) ? (object)DBNull.Value : searchStores.Location);
                    command.Parameters.AddWithValue("@City", string.IsNullOrEmpty(searchStores.City) ? (object)DBNull.Value : searchStores.City);
                    command.Parameters.AddWithValue("@State", string.IsNullOrEmpty(searchStores.State) ? (object)DBNull.Value : searchStores.State);
                    try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var store = new StoreListDTO
								{
									StoreId = Convert.ToString(reader["StoreIdPk"]),
									StoreCode = Convert.ToString(reader["StoreCode"]),
									Location = Convert.ToString(reader["Location"]),
									City = Convert.ToString(reader["City"]),
									State = Convert.ToString(reader["State"])
								};

								storeList.Add(store);
							}
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return storeList;
		}


		public async Task<bool> AddStoreAsync(AddStoreDTO store)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			string query = @"
        INSERT INTO Store (
            StoreCode, StoreName, Location, ContactNumber,
            City, State, ManagerName, ManagerContactNumber,
            IsCorporateOffice, CreatedBy, CreatedOn)
        VALUES (
            @StoreCode, @StoreName, @Location, @ContactNumber,
            @City, @State, @ManagerName, @ManagerContactNumber,
            @IsCorporateOffice, @CreatedBy, @CreatedOn)";

			using SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@StoreCode", store.StoreCode);
			cmd.Parameters.AddWithValue("@StoreName", store.StoreName);
			cmd.Parameters.AddWithValue("@Location", store.StoreLocation);
			cmd.Parameters.AddWithValue("@ContactNumber", store.ContactNumber);
			cmd.Parameters.AddWithValue("@City", store.City);
			cmd.Parameters.AddWithValue("@State", store.State);
			cmd.Parameters.AddWithValue("@ManagerName", store.ManagerName);
			cmd.Parameters.AddWithValue("@ManagerContactNumber", store.ManagerNo);
			cmd.Parameters.AddWithValue("@IsCorporateOffice", store.IsCorporateOffice);
			cmd.Parameters.AddWithValue("@CreatedBy", store.CreatedBy); // Replace if needed
			cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

			int rows = await cmd.ExecuteNonQueryAsync();
			return rows > 0;
		}

        public async Task<StoreEditDTO?> GetStoreByIdAsync(int storeIdPk)
        {
            StoreEditDTO storeEditObj = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = @"SELECT StoreIdPk, StoreCode, Location, ManagerName, ManagerContactNumber, GST, CIN
									FROM Store 
                            WHERE StoreIdPk = @StoreIdPk";
                    command.Connection = conn;
					command.Parameters.AddWithValue("@StoreIdPk", storeIdPk);

					try
					{
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                storeEditObj=new StoreEditDTO();
                                storeEditObj.StoreId = Convert.ToInt32(reader["StoreIdPk"]);
                                storeEditObj.StoreCode = reader["StoreCode"].ToString();
                                storeEditObj.StoreLocation = reader["Location"].ToString();
                                
                                
                                
                                storeEditObj.ManagerName = reader["ManagerName"].ToString();
                                storeEditObj.ManagerNo = reader["ManagerContactNumber"].ToString();
                                storeEditObj.GSTNo = reader["GST"].ToString();
                                storeEditObj.CINNo = reader["CIN"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return storeEditObj;
            }
        }
		public async Task<bool> UpdateStoreAsync(StoreEditDTO storeEditObj)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				await conn.OpenAsync();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = @"
                UPDATE Store 
                SET 
                    StoreCode = @StoreCode, 
                    Location = @Location, 
                    ManagerName = @ManagerName, 
                    ManagerContactNumber = @ManagerContactNumber, 
                    GST = @GST, 
                    CIN = @CIN
                WHERE StoreIdPk = @StoreIdPk";

					command.Connection = conn;

					// Safely handle null values using DBNull.Value
					command.Parameters.AddWithValue("@StoreIdPk", storeEditObj.StoreId);
					command.Parameters.AddWithValue("@StoreCode", storeEditObj.StoreCode ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@Location", storeEditObj.StoreLocation ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@ManagerName", storeEditObj.ManagerName ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@ManagerContactNumber", storeEditObj.ManagerNo ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@GST", storeEditObj.GSTNo ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CIN", storeEditObj.CINNo ?? (object)DBNull.Value);

					try
					{
						int rowsAffected = await command.ExecuteNonQueryAsync();
						return rowsAffected > 0; // Return true only if update was successful
					}
					catch (Exception ex)
					{
						throw ex;
					}
					finally
					{
						await conn.CloseAsync();
					}
				}
			}
		}




	}
}
