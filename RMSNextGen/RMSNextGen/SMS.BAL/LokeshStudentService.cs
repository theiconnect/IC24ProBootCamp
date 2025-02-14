using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class LokeshStudentService
    {
        LokeshStudentRepository _studentRepository;
        public LokeshStudentService(LokeshStudentRepository lokeshStudentRepository)
        {
            _studentRepository = lokeshStudentRepository;
        }
        public async Task<bool> SaveStudent(LokeshStudentRegistrationDTO student)
        {
            return await _studentRepository.SaveStudent(student);
            
        }
    }
}
