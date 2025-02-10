using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult CustomerList()
        {
            return View();
        }
        public IActionResult AddNewCustomer()
        {
            return View();
        }
        public IActionResult EditCustomer()
        {
            return View();
        }
        public IActionResult ViewCustomer()
        {
            return View();
        }
    }
}
