using Microsoft.Data.SqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public class LokeshStudentRepository
    {
        private readonly string _connectionString;

        public LokeshStudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> SaveStudent(LokeshStudentRegistrationDTO obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO LokeshStudent (StudentCode, StudentName, DOB, Gender, Grade, IsTransport, Comments) " +
                                   "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade, @IsTransport, @Comments)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentCode", obj.StudentCode);
                        command.Parameters.AddWithValue("@studentName", obj.StudentName);
                        command.Parameters.AddWithValue("@DOB", obj.DOB);
                        command.Parameters.AddWithValue("@Gender", obj.Gender);
                        command.Parameters.AddWithValue("@Grade", obj.Grade);
                        command.Parameters.AddWithValue("@IsTransport", obj.IsOwnTransport);
                        command.Parameters.AddWithValue("@Comments", obj.Comments);
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
    }
}
