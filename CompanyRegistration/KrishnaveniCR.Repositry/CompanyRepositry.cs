using KrishnaveniCR.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaveniCR.Repositry
{
	public class CompanyRepositry
	{
		public string _connectionString;
		public CompanyRepositry(string connectionString)
		{
			_connectionString = connectionString;
		}
		public CompanyDTO GetCompanyDetails()
		{
			using (SqlConnection conn=new SqlConnection(_connectionString))
			{
				using (SqlCommand command = new SqlCommand("usp_GetCompanyDetails", conn))
				{
					conn.Open();
					command.CommandType = System.Data.CommandType.StoredProcedure;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						CompanyDTO company = new CompanyDTO();
						if (reader.Read())
						{
							company.CompanyIdPk = Convert.ToInt32(reader["CompanyIdPk"]);
							company.CompanyName = Convert.ToString(reader["CompanyName"]);
							company.RegistrationNumber = Convert.ToString(reader["CompanyRegistrationNumber"]);
						}

						if (reader.NextResult())
						{
							company.Addresses = new List<AddressDto>();
							while (reader.Read())
							{
								AddressDto address = new AddressDto();
								address.CompanyAddressIdPk = Convert.ToInt32(reader["CompanyAddressIdPk"]);
								address.AddressType = Convert.ToString(reader["AddressType"]);
								address.HouseNo = Convert.ToString(reader["HouseNo"]);
								address.Street = Convert.ToString(reader["Street"]);
								address.LandMark = Convert.ToString(reader["LandMark"]);
								address.City = Convert.ToString(reader["City"]);
								address.State = Convert.ToString(reader["State"]);
								address.Country = Convert.ToString(reader["Country"]);
								address.ZipCode = Convert.ToString(reader["ZipCode"]);

								company.Addresses.Add(address);
							}
						}

						if (reader.NextResult())
						{
							company.HRDetails = new List<HRDetailDto>();
							while (reader.Read())
							{
								HRDetailDto hrDetail = new HRDetailDto();
								hrDetail.HRDetailIdPk = Convert.ToInt32(reader["HRDetailIdPk"]);
								hrDetail.FirstName = Convert.ToString(reader["FirstName"]);
								hrDetail.LastName = Convert.ToString(reader["LastName"]);
								hrDetail.MiddleName = Convert.ToString(reader["MiddleName"]);
								hrDetail.UserName = Convert.ToString(reader["UserName"]);
								hrDetail.Gender = Convert.ToString(reader["Gender"]);
								hrDetail.Email = Convert.ToString(reader["Email"]);
								hrDetail.DOB = Convert.ToDateTime(reader["DOB"]);

								company.HRDetails.Add(hrDetail);
							}
						}
						return company;
					}

				}
			}



		}

	}

		
	
}
