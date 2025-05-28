using BookXpertAssignment.Models;
using BookXpertAssignment.Models.ModelsUsingEFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookXpertAssignment.DAL
{
	public class EmployeeRepository
	{
		private readonly BookXpertAssignmentDbContext _context;
		public EmployeeRepository(BookXpertAssignmentDbContext context)
		{
			_context = context;
		}
		public List<Employee> GetAllEmployees()
		{
		//	// Fetch employees along with their related State data using Eager Loading

			var employees = _context.Employees
						.Include(e => e.State)  // Include related State entity
						.ToList();

			return employees;
		}
		public List<Employee> GetEmployeesBySearch(string searchName)
		{
			var query = _context.Employees
				.Include(e => e.State)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchName))
			{
				query = query.Where(e => e.Name.ToLower().Contains(searchName.ToLower()));
			}

			return query.ToList();
		}


		public bool DeleteEmployee(int id)
		{
			var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
		public bool AddEmployee(EmployeeDTO employeeDto)
		{
			var employee = new Employee
			{
				Name = employeeDto.Name,
				Designation = employeeDto.Designation,
				DateOfJoin = DateOnly.FromDateTime(employeeDto.DateOfJoin),
				DateOfBirth = DateOnly.FromDateTime(employeeDto.DateOfBirth),
				Salary = employeeDto.Salary,
				StateId = employeeDto.StateId,
				Gender = employeeDto.Gender
			};

			_context.Employees.Add(employee);
			 _context.SaveChanges();
			return true;
		}
		public List<State> GetAllStates()
		{
			return  _context.States.ToList();
		}
		public EmployeeEditDTO GetEmployeeById(int employeeId)
		{
			var emp= _context.Employees
						   .Include(e => e.State) 
						   .FirstOrDefault(e => e.Id == employeeId);
			EmployeeEditDTO editDTOObject = new EmployeeEditDTO();
			editDTOObject.Id = emp.Id;
			editDTOObject.Name = emp.Name;
			editDTOObject.Designation = emp.Designation;
			editDTOObject.DateOfJoin = (DateOnly)emp.DateOfJoin;
			

			editDTOObject.Salary = (decimal)emp.Salary;
			editDTOObject.Gender = emp.Gender;
			editDTOObject.StateId = (int)emp.StateId;
			editDTOObject.DateOfBirth = (DateOnly)emp.DateOfBirth;
			return editDTOObject;
		
		}
		public bool UpdateEmployee(EmployeeEditDTO editDtoObject)
		{
			var existingEmployee = _context.Employees.Find(editDtoObject.Id);
			if (existingEmployee != null)
			{
				existingEmployee.Name = editDtoObject.Name;
				existingEmployee.Designation = editDtoObject.Designation;
				existingEmployee.DateOfJoin = editDtoObject.DateOfJoin;
				existingEmployee.Salary = editDtoObject.Salary;
				existingEmployee.Gender = editDtoObject.Gender;
				existingEmployee.StateId = editDtoObject.StateId;
				existingEmployee.DateOfBirth = editDtoObject.DateOfBirth;

				_context.SaveChanges();
				return true;
			}
			return false;
		}






	}
}
