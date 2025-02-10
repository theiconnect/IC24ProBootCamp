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
using SMS.Models;
using SMS.Services; // Add BCrypt.Net-Next Package using NuGet

namespace SMS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserService _userService; // Inject the UserService

        public UserController(IConfiguration configuration,
            UserService userService)
        {
            this.configuration = configuration;
            _userService = userService;
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
            if (ModelState.IsValid)
            {
                // 1. Use the UserService to authenticate the user
                UserDTO user = await _userService.AuthenticateUser(model.Username, model.Password); // Assuming LoginViewModel.Username holds the email

                if (user != null)
                {
                    // Authentication successful

                    // 2. Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, GetRoleName(user.RoleId))
                    };

                    // 3. Create an identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 4. Create authentication properties
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                    };

                    // 5. Sign in the user
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // 6. Redirect
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
                // Call the UserService to register the user
                bool registrationSuccessful = await _userService.RegisterUser(model.Email, model.Password);

                if (registrationSuccessful)
                {
                    // Registration successful - Redirect to Login
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    // Registration failed (e.g., email already exists)
                    ModelState.AddModelError("", "Registration failed.  Please try again.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }
    }
}
