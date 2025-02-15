using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using SMS.Services;
using SMS.Web.Models;
using System.Reflection;
using SMS.Models;

namespace SMS.Web.Controllers
{
	public class SaikumarController : Controller
	{
		SaiStudentService _saiStudentService;
		public SaikumarController(SaiStudentService saiStudentService)
		{
			_saiStudentService = saiStudentService;
		}
		[HttpGet]
		public IActionResult StudentRegistration()

		{
			SaiStudentViewModel model = new SaiStudentViewModel();
			return View(model);
		}
		
		[HttpPost]
		public  async Task<IActionResult> StudentRegistration(SaiStudentViewModel model)
		{

			SaiStudentRegistrationDTO saiDTO= new SaiStudentRegistrationDTO();
			saiDTO.StudentCode = model.StudentCode;
			saiDTO.StudentName = model.StudentName;
			saiDTO.DOB = model.DOB;
			saiDTO.Gender = model.Gender;
			saiDTO.Grade = model.Grade;
			saiDTO.IsOwnTransport = model.IsOwnTransport;
			saiDTO.Comments = model.Comments;
			
			bool result= await _saiStudentService.SaveStudent(saiDTO);
			ViewBag.Message = result ? "Student Registered Successfully" : "Unable to register the Student";



			return View(model);

		}


	}
	

}
