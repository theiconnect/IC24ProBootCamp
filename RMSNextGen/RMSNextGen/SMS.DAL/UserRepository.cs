using System.Data;
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
            UserDTO user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("dbo.GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            user = new UserDTO
                            {
                                UserId = Convert.ToInt32(dr["UserId"]),
                                Email = Convert.ToString(dr["Email"]),
                                PasswordHash = Convert.ToString(dr["PasswordHash"]),
                                RoleId = Convert.ToInt32(dr["RoleId"])
                            };
                        }
                    }
                }
            }
            return user;
        }

        public async Task<bool> CreateUser(UserDTO user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("dbo.CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    SqlParameter paramResponse = new SqlParameter("@OutResponse", SqlDbType.Int);
                    paramResponse.Direction = ParameterDirection.Output;
                    command.Parameters.Add(paramResponse);

                    await command.ExecuteNonQueryAsync();

                    return Convert.ToInt32(paramResponse.Value) > 0;
                }
            }
        }
    }
}
