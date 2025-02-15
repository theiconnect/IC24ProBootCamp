using Microsoft.Data.SqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public class KrishnaveniStudentRepository
    {
        public string _connectionString;
        public KrishnaveniStudentRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
		public async Task<bool> SaveStudent(KrishnaveniStudentRegistrationDTO obj)
        {
            using(SqlConnection con=new SqlConnection(_connectionString))
            {
                string query= "Insert Into KrishnaveniStudent(StudentCode,StudentName,DOB,Gender,Grade,IsOwnTransport,Comments)" +
					"values(@StudentCode,@StudentName,@DOB,@Gender,@Grade,@IsOwnTransport,@Comments)";
				con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@StudentCode",obj.StudentCode);
                        command.Parameters.AddWithValue("@StudentName",obj.StudentName);
                        command.Parameters.AddWithValue("@DOB",obj.DOB);
                        command.Parameters.AddWithValue("@Gender",obj.Gender);
                        command.Parameters.AddWithValue("@Grade",obj.Grade);
                        command.Parameters.AddWithValue("@IsOwnTransport",obj.IsOwnTransport);
                        command.Parameters.AddWithValue("@Comments",obj.Comments);


                        await command.ExecuteNonQueryAsync();
						return true;
					}
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                

            }
        }


    }
}
