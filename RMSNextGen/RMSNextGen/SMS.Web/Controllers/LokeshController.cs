using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SMS.Web.Controllers
{
    public class LokeshController : Controller
    {
        // GET: LokeshController
        [HttpGet]
        public ActionResult StudentRegistration()
        {
            ViewData["UserName"] = "SamPle ViewData";

            ViewBag.UserName1 = 34545;
            EmployeeViewModel employee = new EmployeeViewModel
            {
                StudentCode = "STD-001",
                Gender = "Male",
                Comments = "Comments1",
                Grade = $"Grade-1",
            };



            return View(employee);
            //ViewBag.Employees = employees;
            //return View(employees);
        }
        [HttpPost]
        public IActionResult StudentRegistration(EmployeeViewModel model)
        {
            return View(model);
        }

        //Parameters
        //[HttpPost]
        //public ActionResult StudentRegistration(string StudentCode, string StudentName, string DOB, string Gender, string Grade, string IsOwnTransport, string Comments)
        //{
        //    return View();
        //}

        private List<EmployeeViewModel> GetDBEmployees()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            for (int i = 0; i < 10; i++)
            {
                EmployeeViewModel employee = new EmployeeViewModel
                {
                    StudentCode = "STD-00" + i,
                    StudentName = "name" + i,
                    DOB = DateTime.Now.AddDays(-i).ToString(),
                    Gender = "Male",
                    Comments = "Comments" + i,
                    Grade = $"Grade {i}",
                    IsOwnTransport = "on"
                };
                employees.Add(employee);
            }
            return employees;
        }

    }

    public class EmployeeViewModel
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Grade { get; set; }
        public string IsOwnTransport { get; set; }
        public string Comments { get; set; }
    }
}
