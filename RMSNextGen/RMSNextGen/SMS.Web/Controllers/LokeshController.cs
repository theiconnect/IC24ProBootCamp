using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Services;
using SMS.Web.Models;
using System.Xml.Linq;

namespace SMS.Web.Controllers
{
    public class LokeshController : Controller
    {
        LokeshStudentService _lokeshStudentService;
        public LokeshController(LokeshStudentService lokeshStudentService)
        {
            _lokeshStudentService = lokeshStudentService;
        }

        [HttpGet]
        public ActionResult StudentRegistration()
        {
            LokeshStudentViewModel model = new LokeshStudentViewModel();
            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> StudentRegistration(LokeshStudentViewModel model)
        {
            LokeshStudentRegistrationDTO student = new LokeshStudentRegistrationDTO();
            student.StudentCode = model.StudentCode;
            student.StudentName = model.StudentName;
            student.Gender = model.Gender;
            student.DOB = model.DOB;
            student.Comments= model.Comments;
            student.IsOwnTransport= model.IsOwnTransport;
            student.Grade = model.Grade;

           bool result = await _lokeshStudentService.SaveStudent(student);
            ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
            return View(model);

        }

        
    }

   
}
