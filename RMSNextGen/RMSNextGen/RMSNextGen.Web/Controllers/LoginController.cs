using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendOtp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
