using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System.Collections.Generic;



namespace RMSNextGen.DAL
{
	public class StoreRepository
	{
		public string connectionstring;


		public StoreRepository(string connectionstring)
		{
			this.connectionstring = connectionstring;
		}

		public async Task<bool> AddStore(AddStoreDTO objectdto)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionstring))
				{
					await conn.OpenAsync();

					string query = "Insert into Store(StoreCode,StoreLocation,NickName,Address,OfficeNo,ManagerName,ManagerNo,GSTNo,CINNo,FAX,IsCorporateOffice,CreatedBy,StoreName,ContactNumber)" +
					"Values(@StoreCode,@StoreLocation,@NickName,@Address,@OfficeNo,@ManagerName,@ManagerNo,@GSTNo,@CINNo,@FAX,@IsCorporateOffice,@CreatedBy,@StoreName,@ContactNumber)";

					using (SqlCommand cmd = new SqlCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@StoreCode", objectdto.StoreCode);
						cmd.Parameters.AddWithValue("@StoreLocation", objectdto.StoreLocation);
						cmd.Parameters.AddWithValue("@NickName", objectdto.NickName);
						cmd.Parameters.AddWithValue("@StoreName", objectdto.StoreName);
						cmd.Parameters.AddWithValue("@ContactNumber", objectdto.ContactNumber);
						cmd.Parameters.AddWithValue("@Address", objectdto.Address);
						cmd.Parameters.AddWithValue("@OfficeNo", objectdto.OfficeNo);
						cmd.Parameters.AddWithValue("@ManagerName", objectdto.ManagerName);
						cmd.Parameters.AddWithValue("@ManagerNo", objectdto.ManagerNo);
						cmd.Parameters.AddWithValue("@GSTNo", objectdto.GSTNo);
						cmd.Parameters.AddWithValue("@CINNo", objectdto.CINNo);
						cmd.Parameters.AddWithValue("@FAX", objectdto.FAX);
						cmd.Parameters.AddWithValue("@IsCorporateOffice", objectdto.IsCorporateOffice);
						cmd.Parameters.AddWithValue("@CreatedBy", objectdto.CreatedBy);
						cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Today);
						await cmd.ExecuteNonQueryAsync();
						return true;
					}

				}
			}
			catch (Exception ex)

			{

				return false;

			}



		}
		public List<StoreListDTO> GetStores()
		{
			List<StoreListDTO> storelistobj = new List<StoreListDTO>();

			using (SqlConnection connection = new SqlConnection(connectionstring))
			{
				connection.Open();

				string query1 = "Select StoreIdPk,StoreCode,StoreLocation,City,State from Store";

				using SqlCommand command = new SqlCommand(query1, connection);
				try
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							StoreListDTO obj = new StoreListDTO();
							obj.StoreId = Convert.ToString(reader["StoreIdPk"]);

							obj.StoreCode = Convert.ToString(reader["StoreCode"]);

							obj.Location = Convert.ToString(reader["StoreLocation"]);

							obj.City = Convert.ToString(reader["City"]);

							obj.State = Convert.ToString(reader["State"]);


							storelistobj.Add(obj);




						}
					}
				}
				catch (Exception ex)
				{
					throw ex;

				}
			}
			return storelistobj;

		}


		public List<StateDTO> GetStates()
		{
			List<StateDTO> stateDTOobj = new List<StateDTO>();

			using SqlConnection connection = new SqlConnection(connectionstring);
			{
				connection.Open();

				string query1 = "Select StateId,Name from StateMaster";

				using SqlCommand command = new SqlCommand(query1, connection);

				try
				{
					using SqlDataReader reader = command.ExecuteReader();
					{
						while (reader.Read())
						{
							StateDTO obj = new StateDTO();

							obj.StateId = Convert.ToInt32(reader["StateId"]);

							obj.Name = Convert.ToString(reader["Name"]);

							stateDTOobj.Add(obj);

						}

					}
				}

				catch (Exception ex)
				{
					throw ex;
				}

			}
			return stateDTOobj;

		}

		public List<CityDTO> GetCities(int stateId)
		{
			List<CityDTO> cityDTOobj = new List<CityDTO>();

			using SqlConnection connection = new SqlConnection(connectionstring);
			{
				connection.Open();

				string CityQuery = "Select CityId,Name  from CityMaster where StateId = @StateId ";


				using SqlCommand command = new SqlCommand(CityQuery, connection);

				command.Parameters.AddWithValue("@StateId", stateId);
				try
				{
					using SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						CityDTO obj = new CityDTO();
						obj.CityID = Convert.ToInt32(reader["CityId"]);
						obj.Name = Convert.ToString(reader["Name"]);

						cityDTOobj.Add(obj);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return cityDTOobj;

		}
		//public List<SearchStoresDTO> SearchStores()

		//{
		//	List<SearchStoresDTO> searchStoresDTOs = new List<SearchStoresDTO>();
		//	return searchStoresDTOs;
		//	using (SqlConnection connection = new SqlConnection(connectionstring))
		//	{
		//		connection.Open();

		//		string query = "Select StoreCode,StoreLocation,City,State from Store" +
		//			"where (@StoreCode is null or StoreCode=@StoreCode) and (@StoreLocation is null or StoreLocation=@StoreLocation) and(@City is null or City=@City)" +
		//			"and(@State is null or State=@State)";

		//		using (SqlCommand command = new SqlCommand(query, connection))
		//		{

		//			command.Parameters.Add("@StoreCode", searchStores.StoreCode);

		//			command.Parameters.Add("@StoreName", searchStores.StoreCode);


		//			command.Parameters.Add("@State", searchStores.StoreCode);


		//			command.Parameters.Add("@City", searchStores.StoreCode);

		//		}


		//	}
	}
}







