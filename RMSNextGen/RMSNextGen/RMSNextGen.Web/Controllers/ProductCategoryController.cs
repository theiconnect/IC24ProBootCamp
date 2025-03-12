using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
    public class ProductCategoryController : Controller
    {


		ProductCategoryServices _ProductCategoryServices;

		public string CreatedBy = "vijay";
		public ProductCategoryController(ProductCategoryServices ProductCategoryServices)
		{
			_ProductCategoryServices = ProductCategoryServices;
		}

			[HttpGet]
        public IActionResult CategoryList(SearchViewModel model)

        {
			CategorySearchDTO search = new CategorySearchDTO();

			search.CategoryCode = model.CategoryCode;
			search.CategoryName = model.CategoryName;

          
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

		public IActionResult Add()

		{
			return View();
		}
		
		[HttpPost]

		public async Task<IActionResult> AddCategory(ProductCategoryViewModel model)
		{
			ProductCategoryDTO obj = new ProductCategoryDTO();
			obj.CategoryIdPK = model.CategoryIdPK;
			obj.CategoryCode = model.CategoryCode;
			obj.CategoryName = model.CategoryName;
			obj.Description = model.Description;
			obj.CreatedBy = CreatedBy;
			obj.CreatedOn = model.CreatedOn;
			
			bool result = await _ProductCategoryServices.AddCategory(obj);

			ViewBag.Response = result;

			//ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
			return View(model);
		}

		public IActionResult ViewCategory()
        {
            return View();
        }


    }
}
