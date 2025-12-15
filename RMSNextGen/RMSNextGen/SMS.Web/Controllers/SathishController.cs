using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Services;
 

namespace SMS.Web.Controllers
{
    public class SathishController : Controller
    {
        SathishStudentServices _SathishStudentServices;
        public SathishController(SathishStudentServices SathishStudentServices)
        {
            _SathishStudentServices = SathishStudentServices;
        }
        public IActionResult StudentRegistration()
        {
            ViewData["USERNAME"] = "StudentData";
            ViewBag.PASS = 64545;
            return View();
            SathishStudentDetalisModel model = new SathishStudentDetalisModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> StudentRegistration(SathishStudentDetalisModel model)
        {
            SathishStudentRegistrationDTO obj = new SathishStudentRegistrationDTO();
            obj.StudentCode = model.StudentCode;
            obj.StudentName = model.StudentName;
            obj.Gender = model.Gender;
            obj.DOB = model.DOB;
            obj.Grade = model.Grade;
            bool result = await _SathishStudentServices.SaveStudent(obj);
            ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
            return View(model);
        }
    }

    public class SathishStudentDetalisModel
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string DOB { get; set; }
        public string Grade { get; set; }
        public string Gender { get; set; }
    }
}