using Microsoft.Data.SqlClient;
using SMS.Models;

namespace SMS.DAL
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sql = "SELECT UserId, Email, PasswordHash, RoleId FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                return new UserDTO
                                {
                                    UserId = (int)reader["UserId"],
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    RoleId = (int)reader["RoleId"]
                                };
                            }
                            else
                            {
                                //User not found with the email
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null; // Or throw the exception after logging
                }
            }
        }

        public async Task<bool> CreateUser(UserDTO user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sql = "INSERT INTO Users (Email, PasswordHash, RoleId) VALUES (@Email, @PasswordHash, @RoleId)"; // Adjust

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        command.Parameters.AddWithValue("@RoleId", user.RoleId);

                        await command.ExecuteNonQueryAsync();
                    }

                    return true; // Indicate success
                }
                catch
                {
                    return false; // Or throw the exception after logging
                }
            }
        }
    }
}
