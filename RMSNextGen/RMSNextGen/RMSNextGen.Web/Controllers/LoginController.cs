using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using RMSNextGen.Services;
using Newtonsoft.Json.Linq;
using RMSNextGen.Models;

namespace RMSNextGen.Web.Controllers
{

	public class LoginController : RMSBaseController
    {
		private readonly IConfiguration _configuration;

		LoginServices _loginServices;
		public LoginController(IConfiguration configuration, LoginServices LoginServices)
		{
			_configuration = configuration;

			_loginServices = LoginServices;

		}
		[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
    {
            //ViewData["Title"] = "Sign In";
            //ViewData["UserName"] = "Murali";
            return View();  
        }
        [HttpPost]
        [AllowAnonymous]
		public  IActionResult Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				foreach (var entry in ModelState)
				{
					var key = entry.Key;
					var errors = entry.Value.Errors;

					foreach (var error in errors)
					{
						Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
					}
				}

			
				return View(model);
			}
			UserDTO userDto = new UserDTO();
			userDto.Email= model.Email;
			userDto.PasswordHash = model.Password;

			//var user = await _loginServices.LoginAsync(userDto);
			var user =  _loginServices.AuthenticateUser(model.Email, model.Password);
			//ViewBag.UserRole = user.RoleName;
			if (user == null)
			{
				ViewBag.Error = "Invalid credentials";
				return View(model);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, model.Email),
				new Claim(ClaimTypes.Email, model.Email),
				//new Claim(ClaimTypes.Role,user.RoleName)
		    };
				//ClaimsIdentity = Who the user is + which method was used (cookies).
				//ClaimsPrincipal = Final object that holds the user's identity + roles.

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
				//This line logs the user in by creating a cookie and storing it in the browser.
			 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				//After successful login, send the user to the view.
			return RedirectToAction("StoreList", "Store");


		}

		//[HttpPost]
		//public IActionResult Login(LoginViewModel model)
		//{
		//	if (!ModelState.IsValid)
		//              return View(model);

		//	var user = _loginServices.AuthenticateUser(model.Email, model.Password);

		//	if (user == null)
		//	{
		//		ViewBag.Error = "Invalid credentials";
		//		return View(model);
		//	}

		//	var claims = new[]
		//	{
		//		new Claim(ClaimTypes.Name, user.Email),
		//		new Claim(ClaimTypes.Email, user.Email),
		//	};

		//	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		//	var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//	var token = new JwtSecurityToken(
		//		issuer: _configuration["Jwt:Issuer"],
		//		audience: _configuration["Jwt:Audience"],
		//		claims: claims,
		//		expires: DateTime.Now.AddHours(1),
		//		signingCredentials: creds
		//	);

		//	var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

		//	//HttpContext.Session.SetString("JWToken", jwtToken);


		//	return RedirectToAction("StoreList", "Store");

		//}
		[HttpGet]
		public IActionResult AccessDenied()
        {
            return View();
        }


        //[HttpPost]
        //public IActionResult Login(IFormCollection form)
        //{
        //    //TODO: get username pasword and verify in the db...
        //    return RedirectToAction("StoreList", "Store");
        //}


        [HttpGet]
        public IActionResult SendOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendOtp(IFormCollection form)
        {
            return RedirectToAction("VerifyOtp", "Login");
        }
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(IFormCollection form)
        {
            return RedirectToAction("ResetPassword", "Login");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(IFormCollection form)
        {
            return RedirectToAction("Login", "Login");
        }
    }
}
