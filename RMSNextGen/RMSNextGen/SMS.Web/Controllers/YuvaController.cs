using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Services;
using SMS.Web.Models;


namespace SMS.Web.Controllers
{
	public class YuvaController : Controller
	{
        YuvaStudentRegistratonService _studentService;

	 public YuvaController(YuvaStudentRegistratonService StudentService)
		{
			_studentService = StudentService;

        }
        public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult StudentRegistration()
		{
			return View();
		}

		[HttpPost]
		public async Task <IActionResult> StudentRegistration(YuvaStudentRegistrationViewModel model)
		{
			YuvaStudentRegistrationDTO Student = new YuvaStudentRegistrationDTO();
            Student.StudentCode=model.StudentCode;
            Student.StudentName = model.StudentName;
            Student.DOB = model.DOB;
            Student.Gender = model.Gender;
            Student.Grade = model.Grade;
            Student.IsOwnTransport = model.IsOwnTransport;
            Student.Comments = model.Comments;

            bool Result = await _studentService.YuvaStudent(Student);
			ViewBag.message = Result ? "StudentRegisteredSuccessfully" : "Unable To  Register The Student";

            return View(model);
		}
	}
}

