using Microsoft.AspNetCore.Mvc;

namespace SMS.Web.Controllers
{
    public class SathishController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StudentRegistration()
        {
            return View();
        }
    }
}
