using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Web.Models;
using SMS.Services;

namespace SMS.Web.Controllers
{
	public class VijayKumarController : Controller
	{
		GVijayStudentService _GVijayStudentService;
		public VijayKumarController(GVijayStudentService GVijayStudentService)
		{
			_GVijayStudentService = GVijayStudentService;
		}

		
		[HttpGet]
		public IActionResult StudentRegistration()
		{
			GVijayStudentModel model = new GVijayStudentModel();
			return View(model);

		}
		[HttpPost]

		public async Task<IActionResult> StudentRegistration(GVijayStudentModel model)
		{
			GVijayStudentRgistrationDTO student = new GVijayStudentRgistrationDTO();
			student.StudentCode = model.StudentCode;
			student.StudentName = model.StudentName;
			student.Gender = model.Gender;
			student.DOB = model.DOB;
			student.Comments = model.Comments;
			student.IsOwnTransport = model.IsOwnTransport;
			student.Grade = model.Grade;
			bool result = await _GVijayStudentService.SaveStudentDetails(student);

			ViewBag.Response = result;

			//ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
			return View(model);

		
			
		}
		 



	}

	
}
