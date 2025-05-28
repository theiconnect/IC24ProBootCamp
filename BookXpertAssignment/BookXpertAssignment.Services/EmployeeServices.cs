using BookXpertAssignment.DAL;
using BookXpertAssignment.Models;
using BookXpertAssignment.Models.ModelsUsingEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookXpertAssignment.Services
{
	public class EmployeeServices
	{
		EmployeeRepository _employeeRepository;
		public EmployeeServices(EmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}
		public List<Employee> GetEmployees(string searchName)
		{
			if (string.IsNullOrWhiteSpace(searchName))
				return _employeeRepository.GetAllEmployees();
			else
			return _employeeRepository.GetEmployeesBySearch(searchName);
		}


		public bool DeleteEmployee(int id)
		{
			return _employeeRepository.DeleteEmployee(id);
		}
		public List<State> GetAllStates()
		{
			return _employeeRepository.GetAllStates();
		}

		public bool AddEmployee(EmployeeDTO employee)
		{
			return _employeeRepository.AddEmployee(employee);

		}
		public EmployeeEditDTO GetEmployeeById(int employeeId)
		{
			return _employeeRepository.GetEmployeeById(employeeId);
		}
		public bool UpdateEmployee(EmployeeEditDTO editDtoObject)
		{
			return _employeeRepository.UpdateEmployee(editDtoObject);
		}








	}
}
