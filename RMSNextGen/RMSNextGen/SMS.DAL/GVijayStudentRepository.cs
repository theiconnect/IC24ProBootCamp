using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using SMS.Models;

namespace SMS.DAL
{
   public class GVijayStudentRepository
    {

		public string _connectionstring;
		public GVijayStudentRepository(string connectionstring)
		{
			_connectionstring = connectionstring;
		}

		public async Task<bool>  SaveStudentDetails(GVijayStudentRgistrationDTO obj)
        {

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionstring))
				{
					await connection.OpenAsync();

					string query = "INSERT INTO GVijayStudent (StudentCode, StudentName, DOB, Gender, Grade, IsOwnTransport, Comments) " +
					 "VALUES (@StudentCode, @StudentName, @DOB, @Gender, @Grade, @IsOwnTransport, @Comments)";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						
						command.Parameters.AddWithValue("@StudentCode", obj.StudentCode);
						command.Parameters.AddWithValue("@StudentName", obj.StudentName);
						command.Parameters.AddWithValue("@DOB", obj.DOB);
						command.Parameters.AddWithValue("@Grade", obj.Grade);
						command.Parameters.AddWithValue("@Gender", obj.Gender);
						command.Parameters.AddWithValue("@IsOwnTransport", obj.IsOwnTransport);
						command.Parameters.AddWithValue("@Comments ", obj.Comments);

						
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
