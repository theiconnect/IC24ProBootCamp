using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Models;

namespace SMS.Services
{
    public class SathishStudentServices
    {
        SathishStudentRepositary _studentRepository;
        public SathishStudentServices(SathishStudentRepositary SathishStudentRepositary)
        {
            _studentRepository = SathishStudentRepositary;
        }
        public async Task<bool> SaveStudent(SathishStudentRegistrationDTO student)
        {
            return await _studentRepository.SaveStudent(student);

        }
    }
}
