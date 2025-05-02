using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RMSAppUsingWebAPI.DAL;
using RMSAppUsingWebAPI.Models;
using RMSNextGen.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMSAppUsingWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginApiController : Controller
	{
		private readonly LoginApiRepository _loginApiRepository;
		private readonly JwtSettings _jwtSettings;

		public LoginApiController(LoginApiRepository LoginApiRepository, IOptions<JwtSettings> jwtSettings)
		{
			_loginApiRepository = LoginApiRepository;
			_jwtSettings=jwtSettings.Value;// Access JWT settings

		}
		[HttpGet]
		public IActionResult Login()
		{
			return Ok("Use POST to Login");
		}
		[HttpPost("Login")]
		public IActionResult Login([FromBody] UserDTO loginDto)
		{
			// 1.Validate credentials

			var user = _loginApiRepository.GetUserByEmail(loginDto.Email);
			if (user == null || user.PasswordHash != loginDto.PasswordHash)
			{
				return Unauthorized("Invalid credentials");
			}

			// 2. Generate JWT token
			var token = GenerateJwtToken(user);

			return Ok(new { Token = token });
		}
		//Generate JWT Token
		private string GenerateJwtToken(UserDTO user)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role,user.RoleName)
				
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}



	}
}
