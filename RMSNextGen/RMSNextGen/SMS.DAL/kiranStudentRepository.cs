using Microsoft.Data.SqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SMS.DAL
{
   public class kiranStudentRepository
    {
		private readonly string _connectionString;

		public kiranStudentRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
		public async Task<bool> SaveStudentdetails(kiranStudentDTO DTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{


					string Query = "insert into kiranstudent(StudentCode, StudentName, DOB, Gender, Grade, Comments, IsOwnTransport)" +
							 "values(@StudentCode, @StudentName, @DOB, @Gender, @Grade, @Comments, @IsOwnTransport)";
					connection.Open();
					using (SqlCommand command = new SqlCommand(Query, connection))
					{
						command.Parameters.AddWithValue("@StudentCode", DTO.StudentCode);
						command.Parameters.AddWithValue("@StudentName", DTO.StudentName);
						command.Parameters.AddWithValue("@DOB", DTO.DOB);
						command.Parameters.AddWithValue("@Gender", DTO.Gender);
						command.Parameters.AddWithValue("@Grade", DTO.Grade);
						command.Parameters.AddWithValue("@Comments", DTO.Comments);
						command.Parameters.AddWithValue("@IsOwnTransport", DTO.IsOwnTransport);

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
