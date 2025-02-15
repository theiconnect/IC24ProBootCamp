using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;

using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
    public class EmployeeController : Controller
	{
		EmployeeService _employeeservice;
		public EmployeeController(EmployeeService employeeservice) 
		{
			_employeeservice = employeeservice;
		}
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
		[HttpPost]
		public async Task<IActionResult> AddNewEmployee(EmployeeViewModel model)
		{

			EmployeeDTO EmpDTO = new EmployeeDTO();
			EmpDTO.EmployeeCode = model.EmployeeCode;
			EmpDTO.EmployeeName = model.EmployeeName;
			EmpDTO.StoreCode = model.StoreCode;
			EmpDTO.Role = model.Role;
			EmpDTO.Gender = model.Gender;
			EmpDTO.Salary = model.Salary;
			EmpDTO.ContactNumber = model.ContactNumber;
			EmpDTO.Addressline1 = model.Addressline1;
			EmpDTO.Addressline2 = model.Addressline2;
			EmpDTO.City = model.City;
			EmpDTO.State = model.State;
			EmpDTO.Pincode = model.Pincode;
			bool result = await _employeeservice.SaveEmployee(EmpDTO);
			ViewBag.Message = result ? "Student Registered Successfully" : "Unable to register the Student";



			return View(model);

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
