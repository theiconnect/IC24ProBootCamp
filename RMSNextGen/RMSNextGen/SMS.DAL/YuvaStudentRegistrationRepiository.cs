using Microsoft.Data.SqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
   public   class YuvaStudentRegistrationRepiository
    {
        public string connectionString;

        public YuvaStudentRegistrationRepiository( string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<bool> SaveStudent(YuvaStudentRegistrationDTO student)

        {

            try

            {

                using (SqlConnection connection = new SqlConnection(connectionString))

                {

                    await connection.OpenAsync();

                    string query = "INSERT INTO YuvaStudent (StudentCode, StudentName, DOB, Gender, Grade, IsOwnTransport, Comments) " +

                                   "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade, @IsOwnTransport, @Comments)";

                    using (SqlCommand command = new SqlCommand(query, connection))

                    {

                        command.Parameters.AddWithValue("@StudentCode", student.StudentCode);

                        command.Parameters.AddWithValue("@studentName", student.StudentName);

                        command.Parameters.AddWithValue("@DOB", student.DOB);

                        command.Parameters.AddWithValue("@Gender", student.Gender);

                        command.Parameters.AddWithValue("@Grade", student.Grade);

                        command.Parameters.AddWithValue("@IsOwnTransport", student.IsOwnTransport);

                        command.Parameters.AddWithValue("@Comments", student.Comments);

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

