using Microsoft.AspNetCore.Mvc;
using SMS.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using BCrypt.Net;
using SMS.Web.Helpers; // Add BCrypt.Net-Next Package using NuGet

namespace SMS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sql = "SELECT UserId, Email, PasswordHash, RoleId FROM Users WHERE Email = @Email"; // Use Email for login
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", model.Username); // Assuming LoginViewModel.Username stores email

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                int userId = Convert.ToInt32(reader["UserId"]);
                                string email = reader["Email"].ToString();    // Get the email
                                string hashedPassword = reader["PasswordHash"].ToString(); // Get the BCrypt hash from the database
                                int roleId = Convert.ToInt32(reader["RoleId"]);

                                // 2. Verify the password using BCrypt
                                if (PasswordHelper.VerifyPassword(model.Password, hashedPassword))
                                {
                                    // Authentication successful

                                    // 3. Create claims
                                    var claims = new List<Claim>
                                        {
                                            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Use UserId for NameIdentifier
                                            new Claim(ClaimTypes.Name, email), // User Email
                                            new Claim(ClaimTypes.Role, GetRoleName(roleId)) // User role
                                        };

                                    // 4. Create an identity
                                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                    // 5. Create authentication properties
                                    var authProperties = new AuthenticationProperties
                                    {
                                        IsPersistent = model.RememberMe,
                                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                                    };

                                    // 6. Sign in the user
                                    await HttpContext.SignInAsync(
                                        CookieAuthenticationDefaults.AuthenticationScheme,
                                        new ClaimsPrincipal(claimsIdentity),
                                        authProperties);

                                    // 7. Redirect
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Invalid email or password.");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid email or password.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    ModelState.AddModelError("", "An error occurred while connecting to the database.");
                }
            }
            return View(model);
            // If we got this far, something failed, redisplay form
        }

        private string GetRoleName(int roleId)
        {
            // This is a placeholder - REPLACE IT with your actual role retrieval logic
            switch (roleId)
            {
                case 1:
                    return "Admin";
                case 2:
                    return "Student";
                case 3:
                    return "Teacher";
                default:
                    return "User"; // Default role
            }
        }

        [Authorize] // Only authenticated users can access this action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // Example of a Register action (for creating new users)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Hash the password using BCrypt
                string hashedPassword = PasswordHelper.HashPassword(model.Password);

                // 2. Insert the new user into the database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        string sql = "INSERT INTO Users (Email, PasswordHash, RoleId) VALUES (@Email, @PasswordHash, @RoleId)"; // Adjust

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Email", model.Email);
                            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            command.Parameters.AddWithValue("@RoleId", 3); // Default role (e.g., "User" or "Student")

                            await command.ExecuteNonQueryAsync();
                        }

                        // Redirect to login page after successful registration
                        return RedirectToAction("Login", "User");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        ModelState.AddModelError("", "An error occurred during registration.");
                        return View(model); // Redisplay the registration form with error message
                    }
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }
    }
}
