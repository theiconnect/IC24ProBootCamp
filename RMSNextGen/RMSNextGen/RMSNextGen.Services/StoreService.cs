using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RMSNextGen.Services
{
	public class StoreService
	{
		//StoreRepository _storeRepository;
		//public StoreService(StoreRepository addStore)
		//{
		//	_storeRepository= addStore;
		//}
		//public async Task <bool> AddStore(AddStoreDTO Objectdto)
		//{
		//	return await _storeRepository.AddStore(Objectdto);
		//}

		//public   List<StoreListDTO> GetStores()
		//{
		//	return  _storeRepository.GetStores();
		//}

		//public List<StateDTO> GetStates()
		//{
		//	return _storeRepository.GetStates();
		//}

		//public List<CityDTO> GetCities( int stateId)
		//{
		//	return _storeRepository.GetCities(stateId);

		//}

		////public List<SearchStoresDTO> SearchStores(SearchStoresDTO searchdto)
		////{
		////	 return _storeRepository.SearchStores( SearchStores);
		////}
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public StoreService(IHttpContextAccessor httpContextAccessor)
		{
            _httpContextAccessor= httpContextAccessor;
			//_httpClient = new HttpClient();
			//_httpClient.BaseAddress = new Uri("https://localhost:7056/");
			//// Replace with your API base URL
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

			_httpClient = new HttpClient(handler);
			_httpClient.BaseAddress = new Uri("https://localhost:44320/");


		}
		public async Task<List<StoreListDTO>> GetStoresFromApiAsync()
		{
			List<StoreListDTO> stores = new List<StoreListDTO>();
			// Retrieve byte[] from session
			byte[] byteToken;
			bool tokenExists = _httpContextAccessor.HttpContext.Session.TryGetValue("JWToken", out byteToken);

			if (byteToken != null)
			{
				// Convert byte[] back to string
				string token = Encoding.UTF8.GetString(byteToken);

				// Set the Authorization header with the Bearer token
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			
			//HttpResponseMessage response = await _httpClient.GetAsync("api/StoreApi/GetStores");
			string requestUrl = "api/StoreApi/StoreList"; // Endpoint for getting all stores

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);


            if (response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				stores = JsonConvert.DeserializeObject<List<StoreListDTO>>(jsonString);
			}

			return stores;
		}
		public async Task<List<StoreListDTO>> SearchStoresAsync(SearchStoresDTO searchDto)
		{
			List<StoreListDTO> stores = new List<StoreListDTO>();

			string requestUrl = "api/StoreApi/SearchStores?"; // Endpoint for searching stores
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(searchDto.StoreCode))
                queryParams.Add($"storeCode={HttpUtility.UrlEncode(searchDto.StoreCode)}");

            if (!string.IsNullOrEmpty(searchDto.Location))
                queryParams.Add($"storeLocation={HttpUtility.UrlEncode(searchDto.Location)}");

            if (!string.IsNullOrEmpty(searchDto.City))
                queryParams.Add($"city={HttpUtility.UrlEncode(searchDto.City)}");

            if (!string.IsNullOrEmpty(searchDto.State))
                queryParams.Add($"state={HttpUtility.UrlEncode(searchDto.State)}");

            // Combine parameters into a single query string
            if (queryParams.Any())
                requestUrl += string.Join("&", queryParams);
            else
                requestUrl = "api/StoreApi/StoreList"; // No search criteria, return all stores

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                stores = JsonConvert.DeserializeObject<List<StoreListDTO>>(jsonString);
            }

            return stores;
        }

        public async Task<bool> AddStoreAsync(AddStoreDTO store)
		{
            _httpClient.Timeout = TimeSpan.FromMinutes(5); // This will fix the timeout issue

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/StoreApi/AddNewStore", store);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}

            string responseMsg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"API Response: {responseMsg}");

            return false;

        }
        public async Task<StoreEditDTO?> GetStoreByIdAsync(int storeIdPk)
        {
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/StoreApi/GetStoreById/{storeIdPk}");

            if (response.IsSuccessStatusCode)
            {
                //This reads the response as a raw JSON string.
                //string jsonString = await response.Content.ReadAsStringAsync();
                //This line takes that raw JSON string and converts it into your C# object:
                //StoreEditDTO store = JsonSerializer.Deserialize<StoreEditDTO>(jsonString);

                //The above two lines and below line two are same 

                var store = await response.Content.ReadFromJsonAsync<StoreEditDTO>();
                return store;
            }

            string responseMsg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"API Response: {responseMsg}");

            return null;
        }
        public async Task<bool> UpdateStoreAsync(StoreEditDTO storeEditObj)
        {
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/StoreApi/UpdateStore", storeEditObj);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            string responseMsg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"API Response: {responseMsg}");

            return false;
        }






    }


}
