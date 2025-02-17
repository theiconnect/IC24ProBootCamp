using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.DAL;

namespace SMS.Services
{
	public class SaiStudentService
	{
		SaiStudentRepository _studentRepository;
		public SaiStudentService(SaiStudentRepository saiStudentRepository)
		{
			_studentRepository = saiStudentRepository;
		}
		public async Task<bool> SaveStudent(SaiStudentRegistrationDTO saiDTO)
		{
			return await _studentRepository.SaveStudent(saiDTO);
		}
	}
}
