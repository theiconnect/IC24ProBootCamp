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
			EmpDTO.EmployeeFirstName = model.EmployeeFirstName;
			EmpDTO.EmployeeLastName = model.EmployeeLastName;
			EmpDTO.Email = model.Email;
			EmpDTO.MobileNumber = model.MobileNumber;
			EmpDTO.Department = model.Department;
			EmpDTO.Designation = model.Designation;
			EmpDTO.PersonalEmail = model.PersonalEmail;
			EmpDTO.Gender = model.Gender;
			EmpDTO.SalaryCTC = model.SalaryCTC;
			EmpDTO.PermanentAddressline1 = model.PermanentAddressline1;
			EmpDTO.PermanentAddressline2 = model.PermanentAddressline2;
			EmpDTO.PermanentCity = model.PermanentCity;
			EmpDTO.PermanentState = model.PermanentState;
			EmpDTO.PermanentPincode = model.PermanentPincode;
			EmpDTO.CurrentAddressline1 = model.CurrentAddressline1;
			EmpDTO.CurrentAddressline2 = model.CurrentAddressline2;
			EmpDTO.CurrentCity = model.CurrentCity;
			EmpDTO.CurrentState = model.CurrentState;
			EmpDTO.CurrentPincode = model.CurrentPincode;
			EmpDTO.CreatedBy = model.CreatedBy;
			EmpDTO.CreatedOn = model.CreatedOn;
			EmpDTO.LastUpdatedBy = model.LastUpdatedBy;
			EmpDTO.LastUpdatedOn = model.LastUpdatedOn;

			bool result = await _employeeservice.SaveEmployee(EmpDTO);
			//ViewBag.Message = result ? "Student Registered Successfully" : "Unable to register the Student";
			ViewBag.Response = result;
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
