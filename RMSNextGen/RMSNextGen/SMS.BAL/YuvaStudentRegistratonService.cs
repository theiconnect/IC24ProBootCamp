using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class YuvaStudentRegistratonService
    {
        YuvaStudentRegistrationRepiository _yuvaStudentRepository;
        public YuvaStudentRegistratonService(YuvaStudentRegistrationRepiository YuvaStudentRepository)
        {
            _yuvaStudentRepository = YuvaStudentRepository;
        }

        public async Task<bool> YuvaStudent(YuvaStudentRegistrationDTO Student)
        {
            return await _yuvaStudentRepository.SaveStudent(Student);
        }


    }
}
