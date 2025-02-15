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
	public class SaiStudentRepository
	{
		
			private readonly string _connectionString;
			public SaiStudentRepository(string connectionString)
			{
				_connectionString = connectionString;
			}
		public async Task<bool> SaveStudent(SaiStudentRegistrationDTO saiDTO)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{

					string query = "INSERT INTO SaiStudent (StudentCode, StudentName, DOB, Gender, Grade, IsOwnTransport,  Comments) " +
								   "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade, @IsOwnTransport, @Comments)";
					connection.Open();
					using (SqlCommand cmd = new SqlCommand(query,connection))
					{
						cmd.Parameters.AddWithValue("@StudentCode", saiDTO.StudentCode);
						cmd.Parameters.AddWithValue("@StudentName", saiDTO.StudentName);
						cmd.Parameters.AddWithValue("@DOB", saiDTO.DOB);
						cmd.Parameters.AddWithValue("@Gender", saiDTO.Gender);
						cmd.Parameters.AddWithValue("@Grade", saiDTO.Grade);
						cmd.Parameters.AddWithValue("@IsOwnTransport", saiDTO.IsOwnTransport);
						cmd.Parameters.AddWithValue("@Comments", saiDTO.Comments);
						 cmd.ExecuteNonQuery();
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
