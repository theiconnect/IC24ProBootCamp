using BookXpertAssignment.Models;
using BookXpertAssignment.Models.ModelsUsingEFCore;
using BookXpertAssignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookXpertAssignment.Controllers
{
	public class EmployeeController : Controller
	{
		EmployeeServices _employeeservices;
		public EmployeeController(EmployeeServices employeeservices)
		{
			_employeeservices = employeeservices;

		}
		//with out pagination
		//[HttpGet]
		//public IActionResult EmployeeList(string searchName = "")
		//{

		//	 ViewBag.Employee = _employeeservices.GetEmployees(searchName); // returns List<Employee>
		//	return View(); // this sets it as the Model

		//}
		//[HttpPost]
		//[Route("SearchEmployee")]
		//public IActionResult SearchEmployee(string Name)
		//{
		//	ViewBag.Employee = _employeeservices.GetEmployees(Name);
		//	return View("EmployeeList");

		//}

		//with pagination
		[HttpGet]
		[Route("Employee/EmployeeList/{page?}")]

		public IActionResult EmployeeList(int page = 1, string searchName = "")
		{
			int pageSize = 5; // Set number of employees per page

			// Get filtered employees based on the search term
			var allEmployees = _employeeservices.GetEmployees(searchName);
			int totalEmployees = allEmployees.Count();

			// Apply pagination
			var pagedEmployees = allEmployees
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			// Setting ViewBag for pagination and search term
			ViewBag.Employees = pagedEmployees;
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);
			ViewBag.SearchName = searchName;

			return View(pagedEmployees);  // Return paged employees to the view
		}



		//POST: Search for Employees
		[HttpPost]
		[Route("SearchEmployee")]
		public IActionResult SearchEmployee(string Name)
		{
			return RedirectToAction("EmployeeList", new { searchName = Name });
			
		}



		[HttpGet]

		//[Route("Employee/DeleteEmployee/{id}")]

		public IActionResult DeleteEmployee(int id)
		{
			bool isDeleted = _employeeservices.DeleteEmployee(id);

			if (isDeleted)
			{
				TempData["SuccessMessage"] = "Employee deleted successfully.";
			}
			else
			{
				TempData["ErrorMessage"] = "Employee not found.";
			}

			return RedirectToAction("EmployeeList"); // or your listing action
		}
		
		[HttpGet]
		public IActionResult AddNewEmployee()
		{
			var states = _employeeservices.GetAllStates();
			


			ViewBag.States = new SelectList(states, "Id", "Name");

			//SelectList is a class and helps you easily bind a collection to a dropdown list in your view.SelectList(collection,value,text)

			return View();
		}

		
		[HttpPost]
		public  IActionResult AddNewEmployee(EmployeeViewModel model)
		{
			
			EmployeeDTO employee = new EmployeeDTO();
			employee.Name = model.Name;
			employee.Designation = model.Designation;
			employee.DateOfJoin = model.DateOfJoin;
			employee.DateOfBirth = model.DateOfBirth;
			employee.Salary = model.Salary;
			employee.StateId = model.StateId;
			employee.Gender = model.Gender;

			bool result = _employeeservices.AddEmployee(employee);
			ViewBag.Response = result;
			return View(model);
		}
		[HttpGet]
		public IActionResult EditEmployee(int Id)
		{
			EmployeeEditDTO editDTOObject = _employeeservices.GetEmployeeById(Id);
			
			
			EmployeeEditViewModel model=new EmployeeEditViewModel();
			model.Name = editDTOObject.Name;
			model.Designation = editDTOObject.Designation;
			model.DateOfJoin=editDTOObject.DateOfJoin;
			model.StateId=editDTOObject.StateId;
			model.Gender = editDTOObject.Gender;
			model.Salary=editDTOObject.Salary;
			model.DateOfBirth = editDTOObject.DateOfBirth;
			var states = _employeeservices.GetAllStates();
			ViewBag.States = new SelectList(states, "Id", "Name");
			return View(model);


		}
		[HttpPost]
		public IActionResult EditEmployee(EmployeeEditViewModel model)
		{
			EmployeeEditDTO editDtoObject=new EmployeeEditDTO();
			editDtoObject.Id = model.Id;
			editDtoObject.Name = model.Name;
			editDtoObject.Designation=model.Designation;
			editDtoObject.DateOfJoin = model.DateOfJoin;
			editDtoObject.Salary = model.Salary;
			editDtoObject.Gender = model.Gender;
			editDtoObject.DateOfBirth = model.DateOfBirth;
			editDtoObject.StateId=model.StateId;

			bool result=_employeeservices.UpdateEmployee(editDtoObject);
			ViewBag.Response = result;

			return View(model);


		}



	}
}
