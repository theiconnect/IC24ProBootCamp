﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RMSNextGen.DAL;
using RMSNextGen.Models;
using RMSNextGen.Services;

using RMSNextGen.Web.Models;
using System.Reflection;

namespace RMSNextGen.Web.Controllers
{
    public class EmployeeController : RMSBaseController
    {
		EmployeeService _employeeservice;
        LookupService _lookupService;
		public EmployeeController(EmployeeService employeeservice, LookupService lookupService) 
		{
			_employeeservice = employeeservice;
            _lookupService = lookupService;

        }
		
		[HttpGet]
		public async Task<IActionResult> EmployeeList()
		{
            ViewBag.Departments = new SelectList(await _lookupService.GetDepartments(), "DepartmentId", "Department");
            ViewBag.Employees = _employeeservice.GetEmployees(null);

			return View();
		}

        [HttpPost]
        [Route("SearchEmployee")]
        public async Task<IActionResult> SearchEmployee(EmployeeSearchViewModel model)
        {
            var searchObj = new EmployeeSearchDTO(model.EmployeeName, model.EmployeeCode, model.MobileNumber, model.DepartmentId);

            ViewBag.Departments = new SelectList(await _lookupService.GetDepartments(), "DepartmentId", "Department");
            
			ViewBag.Employees = _employeeservice.GetEmployees(searchObj);

            return View("EmployeeList");
        }


		[HttpGet]
        public async Task<IActionResult> EditEmployee(int EmployeeID)
        {
			EmployeeEditDTO employeeEditDTO = new EmployeeEditDTO();
			employeeEditDTO.EmployeeidPK = EmployeeID;
			ViewBag.EmployeeDetails= await _employeeservice.EditEmployee(employeeEditDTO);
			EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel();

		 employeeEditViewModel.EmployeeCode= employeeEditDTO.EmployeeCode;
		employeeEditViewModel.EmployeeFirstName = employeeEditDTO.EmployeeFirstName;
		employeeEditViewModel.EmployeeLastName = employeeEditDTO.EmployeeLastName;
		employeeEditViewModel.Email = employeeEditDTO.Email;
		employeeEditViewModel.MobileNumber = employeeEditDTO.MobileNumber;
		employeeEditViewModel.Department = employeeEditDTO.Department;
		employeeEditViewModel.Designation = employeeEditDTO.Designation;
		employeeEditViewModel.PersonalEmail = employeeEditDTO.PersonalEmail;
		employeeEditViewModel.Gender = employeeEditDTO.Gender;
		employeeEditViewModel.Salary= employeeEditDTO.Salary;
		employeeEditViewModel.PermanentAddressline1 = employeeEditDTO.PermanentAddressLine1;
		employeeEditViewModel.PermanentAddressline2 = employeeEditDTO.PermanentAddressline2;
		employeeEditViewModel.PermanentCity = employeeEditDTO.PermanentCity;
		employeeEditViewModel.PermanentState = employeeEditDTO.PermanentState;
		employeeEditViewModel.PermanentPincode = employeeEditDTO.PermanentPincode;
		employeeEditViewModel.CurrentAddressline1 = employeeEditDTO.CurrentAddressline1;
		employeeEditViewModel.CurrentAddressline2 = employeeEditDTO.CurrentAddressline2;
		employeeEditViewModel.CurrentCity = employeeEditDTO.CurrentCity;
		employeeEditViewModel.CurrentState = employeeEditDTO.CurrentState;
			employeeEditViewModel.CurrentPincode = employeeEditDTO.CurrentPincode;
			
			
			
			return View(employeeEditViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewEmployee()
        {
            ViewBag.Departments = new SelectList(await _lookupService.GetDepartments(), "DepartmentId", "Department");
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
			EmpDTO.DepartmentId = model.DepartmentId;
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
			EmpDTO.UserId = UserName;

			var result = await _employeeservice.SaveEmployee(EmpDTO);
			ViewBag.Message = result.Response.ResponseMessage;
			ViewBag.Response = result.Response.IsSuccess;
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
		
		
    }
}
