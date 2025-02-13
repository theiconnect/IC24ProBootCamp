using Microsoft.AspNetCore.Mvc;

namespace SMS.Web.Controllers
{
    public class VijayController : Controller
    {
        [HttpGet]
        public IActionResult StudentRegistration()
        {
            return View();
        }
    }
}
