using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;



namespace RMSNextGen.Services
{
	public class LoginServices
	{
		LoginRepository _loginRepository;
		//public LoginServices(LoginRepository loginRepository) 
		//{ 
		//	_LoginRepository = loginRepository;
		//}
		//public UserDTO GetUserByEmail(string email)
		//{
		//	return _LoginRepository.GetUserByEmail(email);
		//}
		//public async Task<UserDTO> AuthenticateUser(string email, string password)
		//{
		//	// Step 1: Get user from DB based on email
		//	//UserDTO user = await _LoginRepository.GetUserByEmail(email);

		//	// Step 2: If user exists, verify password
		//	if (user != null )
		//	{
		//		// Step 3: Valid user
		//		return user;
		//	}

		//	// Step 4: Invalid credentials
		//	return null;
		//}
		//

		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LoginServices(IHttpContextAccessor httpContextAccessor, LoginRepository LoginRepository)
		{
			_loginRepository = LoginRepository;
			_httpContextAccessor = httpContextAccessor;
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

			_httpClient = new HttpClient(handler);
			_httpClient.BaseAddress = new Uri("https://localhost:44320/");
			
		}
		//this is repository method without web api
		public UserDTO AuthenticateUser(string email, string password)
		{
			var user = _loginRepository.GetUserByEmail(email);
			if (user != null)
			{
				if (user.PasswordHash == password)
					return user;
				else
					return null;
			}
			return null;
		}

		//this is repository method with web api
		public async Task<bool> LoginAsync(UserDTO userDto)
		{
			string requestUrl = "api/LoginApi/Login"; // Endpoint for Login
		
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, userDto);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var tokenObj = JsonConvert.DeserializeObject<TokenResponseDTO>(content);

				// Store the token in session
				string token = tokenObj.Token;
				byte[] byteToken = Encoding.UTF8.GetBytes(token);  // Convert string to byte[]
				_httpContextAccessor.HttpContext.Session.Set("JWToken", byteToken);  // Store the byte array in session
																					 //decoding token
				if (byteToken != null)
				{
					// Decode byte[] back to string
					string Decodedtoken = Encoding.UTF8.GetString(byteToken);

					
				
				}



				return true;
			}

			return false;
		}




	}
}
