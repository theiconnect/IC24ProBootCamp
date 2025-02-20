using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryList()
        {
            return View();
        }
        public IActionResult EditCategory()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
		[HttpPost]
		public IActionResult AddCategory( AddCategoryViewModel model)
		{
			return View( model);
		}
		public IActionResult ViewCategory()
        {
            return View();
        }


    }
}
