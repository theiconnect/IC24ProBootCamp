using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class CustomersController : Controller
    {
        [HttpGet]
        public IActionResult CustomerList()
        {
            return View();
        }
        [HttpPost]
		public IActionResult CustomerList(IFormCollection form)
		{
			return RedirectToAction("CustomerList", "Customers");
		}



		public IActionResult AddNewCustomer()
        {
            return View();
        }

        public IActionResult EditCustomer()
        {
            return View();
        }
        [HttpGet]
		public IActionResult Save()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Save(IFormCollection form)
		{
			return RedirectToAction("CustomerList", "Customers");
		}


		public IActionResult ViewCustomer()
        {
            return View();
        }
		[HttpGet]
		public IActionResult Back()
		{
			return RedirectToAction("CustomerList", "Customers");
		}
		
		[HttpGet]
		public IActionResult Search()
		{
			return View();
		}
	}
}
