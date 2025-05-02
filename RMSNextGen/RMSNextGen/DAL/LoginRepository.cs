using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
    public class LoginRepository
    {
		public readonly string _connectionString;

		public LoginRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
		public UserDTO GetUserByEmail(string email)
		{
			UserDTO user=null;
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = @"
									SELECT u.Email, u.PasswordHash,r.RoleName
									FROM Users u
									INNER JOIN Role r ON u.RoleId = r.RoleIdPk
									WHERE u.Email = @Email";

					command.Parameters.AddWithValue("@Email", email);
					command.Connection = connection;

					try
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								 user = new UserDTO();
							
								user.Email = reader["Email"].ToString();
								user.PasswordHash = reader["PasswordHash"].ToString();
								
								user.RoleName = Convert.ToString(reader["RoleName"]);


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
			return user;
		}


	}
}
