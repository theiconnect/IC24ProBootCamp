using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Services;
using SMS.Web.Models;

namespace SMS.Web.Controllers
{
    public class VijayController : Controller
    {
        VijayStudentServices _studentService;
        public VijayController(VijayStudentServices studentService)
        {
            this._studentService = studentService;
        }

       [HttpGet]
        public IActionResult StudentRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegistration(VijayStudentViewModels model)
        {
            VijayStudentDTO obj = new VijayStudentDTO();
            obj.Studentcode = model.Studentcode;
            obj.studentName = model.studentName;
            obj.Gender = model.Gender;
            obj.Grade = model.Grade;    
            obj.DOB = model.DOB;
            obj.Comments = model.Comments;
            obj.IsOwnTransport = model.IsOwnTransport;
           bool IsSucces =  await _studentService.SaveStudent(obj);
           ViewBag.Student = IsSucces ? "Student Sucessfully Registration" : "unable to Registration";
            return View(model);
        }
    }
}
