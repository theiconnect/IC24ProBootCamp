using Microsoft.Data.SqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public class VijayStudentRepository
    {
        private readonly string _ConnectionString;
        public VijayStudentRepository(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public async Task<bool> SaveStudent(VijayStudentDTO obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    
                    string query = "INSERT INTO VijayStudent (StudentCode, StudentName, DOB, Gender, Grade, IsOwnTransport, Comments) " +
                                   "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade, @IsOwnTransport, @Comments)";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentCode", obj.Studentcode);
                        command.Parameters.AddWithValue("@studentName", obj.studentName);
                        command.Parameters.AddWithValue("@DOB", obj.DOB);
                        command.Parameters.AddWithValue("@Gender", obj.Gender);
                        command.Parameters.AddWithValue("@Grade", obj.Grade);
                        command.Parameters.AddWithValue("@IsOwnTransport", obj.IsOwnTransport);
                        command.Parameters.AddWithValue("@Comments", obj.Comments);
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            
        }
    }
}
