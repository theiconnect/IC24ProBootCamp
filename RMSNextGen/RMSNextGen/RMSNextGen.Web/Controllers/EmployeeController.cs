using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult EmployeeList()
        {
            return View();
        }
        public IActionResult AddNewEmployee()
        {
            return View();
        }
        public IActionResult EditStore()
        {
            return View();
        }
        public IActionResult ViewStore()
        {
            return View();
        }
    }
}
