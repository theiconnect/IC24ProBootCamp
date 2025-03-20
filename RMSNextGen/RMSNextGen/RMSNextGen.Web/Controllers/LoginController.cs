using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class LoginController : RMSBaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Sign In";
            ViewData["UserName"] = "Murali";
            return View();  
        }


        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            //TODO: get username pasword and verify in the db...
            return RedirectToAction("StoreList", "Store");
        }

         
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
