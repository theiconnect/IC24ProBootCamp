using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
	public class EmployeeService
	{
		EmployeeRepository _employeeRepository;
		public EmployeeService(EmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}
		public async Task<bool> SaveEmployee(EmployeeDTO EmpDTO)
		{
			return await _employeeRepository.SaveEmployee(EmpDTO);
		}
		public List<EmployeeListDTO> GetEmployees(EmployeeSearchDTO searchObj)
		{
			return _employeeRepository.GetEmployees(searchObj);
		}
		public async Task<bool> EditEmployee(EmployeeEditDTO employeeEditDTO)
		{
			return await _employeeRepository.EditEmployee( employeeEditDTO);
		}

	}
}
