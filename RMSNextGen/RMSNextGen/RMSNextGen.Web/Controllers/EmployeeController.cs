using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class EmployeeController : Controller
	{
		[HttpGet]
		public IActionResult EmployeeList()
		{
			//get all stores data and put it in a viewbag/viewdata/model
			return View();
		}

		

		[HttpGet]
        public IActionResult EditEmployee()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewEmployee()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewEmployee()
        {
            return View();
        }
		
		[HttpPost]
		public IActionResult Save(IFormCollection form)
		{
			return RedirectToAction("EmployeeList", "Employee");
		}
		
		[HttpPost]
		public IActionResult Search(IFormCollection form)
		{
			return RedirectToAction("EmployeeList", "Employee");
		}

	}
}
