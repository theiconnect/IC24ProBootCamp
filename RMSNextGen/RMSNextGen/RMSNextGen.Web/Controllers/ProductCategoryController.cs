using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
	public class ProductCategoryController : RMSBaseController
    {


		ProductCategoryServices _ProductCategoryServices;

		public string CreatedBy = "vijay";
		public ProductCategoryController(ProductCategoryServices ProductCategoryServices)
		{
			_ProductCategoryServices = ProductCategoryServices;
		}

		[HttpGet]
		public IActionResult CategoryList()

		{
			CategorySearchDTO searchobj = new CategorySearchDTO();
			ViewBag.Category = _ProductCategoryServices.ProductCategoryList(searchobj);



			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CategoryList(SearchViewModel ProductSearchobj)

		{
			CategorySearchDTO searchobj = new CategorySearchDTO();
			searchobj.CategoryCode = ProductSearchobj.CategoryCode;
			searchobj.CategoryName = ProductSearchobj.CategoryName;

			ViewBag.Category = _ProductCategoryServices.ProductCategoryList(searchobj);
			return View();
		}

		[HttpGet]
		public IActionResult EditCategory(int categoryId)
		{
			EditCategoryDTO editproductcategory = new EditCategoryDTO();
			editproductcategory.ProductCategoryIdPk = categoryId;
			ViewBag.Category = _ProductCategoryServices.EditcategoryId(editproductcategory);

			EditCategoryViewModel Editviewobj = new EditCategoryViewModel();

			Editviewobj.ProductCategoryIdPk = editproductcategory.ProductCategoryIdPk;
			Editviewobj.CategoryCode = editproductcategory.CategoryCode;
			Editviewobj.CategoryName = editproductcategory.CategoryName;
			Editviewobj.Description = editproductcategory.Description;



			return View(Editviewobj);
		}
		[HttpPost]
		public async Task<IActionResult> EditCategory(EditCategoryViewModel Editviewobj)
		{
			EditCategoryDTO editproductcategory = new EditCategoryDTO();
			editproductcategory.ProductCategoryIdPk = Editviewobj.ProductCategoryIdPk;
			editproductcategory.CategoryCode = Editviewobj.CategoryCode;
			editproductcategory.CategoryName = Editviewobj.CategoryName;
			editproductcategory.Description = Editviewobj.Description;

			bool result = await _ProductCategoryServices.Updatecategory(editproductcategory);
			ViewBag.Response = result;
			return View(Editviewobj);


		}

		[HttpGet]
		public IActionResult ViewCategory(int categoryId)
		{
			//get the Product category details from DB of this categoryID
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
			
			
			bool result = await _ProductCategoryServices.AddCategory(obj);

			ViewBag.Response = result;

			
			return View(model);
		}

		public IActionResult ViewCategory()
        {
            return View();
        }


    }
}
