using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RMSNextGen.Models;

namespace RMSNextGen.DAL
{
	public class LookupRepository
	{
		private readonly string _connectionString;
		public LookupRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<DepartmentDto>> GetDepartments()
		{
			List<DepartmentDto> departments = new List<DepartmentDto>() ;
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					using (SqlCommand command = new SqlCommand("usp_GetDepartments", connection))
					{
						command.CommandType = System.Data.CommandType.StoredProcedure;
						connection.Open();
						using (SqlDataReader dr = command.ExecuteReader())
						{
							while (dr.Read())
							{
								DepartmentDto department = new DepartmentDto();
								department.DepartmentId = Convert.ToInt32(dr["DepartmentIdPk"]);
								department.Department = Convert.ToString(dr["Department"]);
                                departments.Add(department);
                            }
						}
						connection.Close();
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}

			return departments;

		}
	}
}



	

