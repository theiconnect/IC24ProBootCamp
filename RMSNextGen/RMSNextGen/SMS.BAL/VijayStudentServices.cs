using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class VijayStudentServices
    {
        VijayStudentRepository _vijayStudent;

        public VijayStudentServices(VijayStudentRepository vijayStudent)
        {
            this._vijayStudent = vijayStudent;  
        }
        public async Task<bool> SaveStudent(VijayStudentDTO obj)
        {
           return await _vijayStudent.SaveStudent(obj);
        }
    }
}
