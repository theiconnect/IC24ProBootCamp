using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace SMS.Web.Controllers
{
    public class PractiseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View("../Views/Home/ChangePassword.cshtml");
        }


    }
}
