using SMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Models;


namespace SMS.Services
{
	public class kiranStudentService
	{
		kiranStudentRepository _studentRepository;
		public kiranStudentService(kiranStudentRepository KiranStudentRepository)
		{
			_studentRepository = KiranStudentRepository;
		}
		public async Task<bool> SaveStudent(kiranStudentDTO DTO)
		{
			return await _studentRepository.SaveStudentdetails(DTO);

		}
	}
}