using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Sign In";
            ViewData["UserName"] = "Murali";
            ViewData["Title"] = "Sign In";
            ViewData["Title"] = "Sign In";
            ViewData["Title"] = "Sign In";
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
