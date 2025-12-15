using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.DAL;
using SMS.Models;

namespace SMS.Services
{
  public  class GVijayStudentService
    {
		GVijayStudentRepository _StudentRepository;
        public GVijayStudentService(GVijayStudentRepository GVijayStudentRepository)
        {
			_StudentRepository = GVijayStudentRepository;
		}
		public async Task<bool> SaveStudentDetails(GVijayStudentRgistrationDTO student)
		{
			return await _StudentRepository.SaveStudentDetails(student);

		}
	}
}
