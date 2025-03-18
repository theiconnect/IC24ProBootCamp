using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class KrishnaveniStudentService
    {
		KrishnaveniStudentRepository _krishnaveniStudentRepository;

		public KrishnaveniStudentService(KrishnaveniStudentRepository krishnaveniStudentRepository)
        {
			_krishnaveniStudentRepository = krishnaveniStudentRepository;
        }
		public async Task<bool> SaveStudent(KrishnaveniStudentRegistrationDTO obj)
		{
			return await _krishnaveniStudentRepository.SaveStudent(obj);

		}
	}
}
