using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SMS.Models;

namespace SMS.Services
{
    public class SathishStudentRepositary
    {
        private readonly string _connectionString;
        public SathishStudentRepositary(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> SaveStudent(SathishStudentRegistrationDTO obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO SathishStudent(StudentCode, StudentName, DOB, Gender, Grade) " +
                                   "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade)";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentCode", obj.StudentCode);
                        command.Parameters.AddWithValue("@studentName", obj.StudentName);
                        command.Parameters.AddWithValue("@DOB", obj.DOB);
                        command.Parameters.AddWithValue("@Gender", obj.Gender);
                        command.Parameters.AddWithValue("@Grade", obj.Grade);
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