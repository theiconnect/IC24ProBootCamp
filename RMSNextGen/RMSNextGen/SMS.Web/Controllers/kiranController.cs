using Microsoft.AspNetCore.Mvc;
using SMS.Web.Models;
using SMS.Models;
using SMS.Services;

namespace SMS.Web.Controllers
{
    
    public class kiranController : Controller
    {
		kiranStudentService _KiranStudentService;
		public kiranController(kiranStudentService KiranStudentService)
		{
			_KiranStudentService = KiranStudentService;
		}
		[HttpGet]
        public IActionResult StudentRegistration()
        {

			return View();
        }
		[HttpPost]
		public async Task<IActionResult> StudentRegistration(kiranStudentViewModel model)
        {
			kiranStudentDTO DTO = new kiranStudentDTO();
			DTO.StudentCode = model.StudentCode;
			DTO.StudentName = model.StudentName;
			DTO.DOB = model.DOB;
			DTO.Gender = model.Gender;
			DTO.Grade = model.Grade;
			DTO.IsOwnTransport = model.IsOwnTransport;
			DTO.Comments = model.Comments;

			bool result = await _KiranStudentService.SaveStudent(DTO);
			ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
			return View();
		}        
           
        

    }
}
