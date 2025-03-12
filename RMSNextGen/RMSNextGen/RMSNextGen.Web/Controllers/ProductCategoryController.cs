using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
     
        [HttpGet]
        public IActionResult CategoryList()

        {

            return View();
        }

        [HttpGet]
        public IActionResult EditCategory()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult UpdateProductCategory(IFormCollection form)
        {
            return RedirectToAction("CategoryList", "ProductCategory");
        }

		[HttpGet]
		public IActionResult AddCategory()
        {
            return View();
        }
		[HttpPost]
		public IActionResult AddCategory(IFormCollection form)
		{
			return RedirectToAction("CategoryList", "ProductCategory");
		}

		public IActionResult ViewCategory()
        {
            return View();
        }


    }
}
