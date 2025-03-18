using Microsoft.AspNetCore.Mvc;
using SMS.Web.Models;
using SMS.Models;
using SMS.Services;
namespace SMS.Web.Controllers
{
    public class KrishnaveniController : Controller
    {
		KrishnaveniStudentService _krishnaveniStudentService;
		public KrishnaveniController(KrishnaveniStudentService krishnaveniStudentService) 
		{
			_krishnaveniStudentService= krishnaveniStudentService;
		}
        [HttpGet]
        public IActionResult StudentRegistration()
        {
			//ViewData["StudentName"] = 3546;
			//ViewBag.StudentLastName = "thota";
			return View();

			//EmployeeViewModel model = new EmployeeViewModel();
			//model.StudentCode = "hgyt";
			//return View(model);
        }
		//[HttpPost]
		//public IActionResult StudentRegistration(string name)
		//{
		//	string studentcode = Request.Form["StudentCode"];
		//	return View();
		//}
		//Parameters
		//[HttpPost]
		//public IActionResult StudentRegistration(string StudentCode, string StudentName, string DOB, string Gender, string Grade, string IsOwnTransport, string Comments)
		//{
		//	return View();
		//}
		[HttpPost]
		public async Task<IActionResult> StudentRegistration(KrishnaveniStudentViewModel model)
		{
			KrishnaveniStudentRegistrationDTO obj = new KrishnaveniStudentRegistrationDTO();
			obj.StudentCode = model.StudentCode;
			obj.StudentName=model.StudentName;
			obj.DOB = model.DOB;
			obj.Gender = model.Gender;
			obj.Grade = model.Grade;
			obj.IsOwnTransport = model.IsOwnTransport;
			obj.Comments = model.Comments;

			bool result = await _krishnaveniStudentService.SaveStudent(obj);
			ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register student";


			return View(model);
		}
		
	}

	//public class KrishnaveniStudentViewModel
	//{
	//	public string StudentCode { get; set; }
	//	public string StudentName { get; set; }
	//	public string DOB { get; set; }
	//	public string Gender { get; set; }
	//	public string Grade { get; set; }
	//	public string IsOwnTransport { get; set; }
	//	public string Comments { get; set; }
	//}
}
