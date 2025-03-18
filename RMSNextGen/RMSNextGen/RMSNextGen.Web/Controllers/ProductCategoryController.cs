using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        string UserName = "Sathish";
        ProductCategoryServices _ProductCategoryServices;

        public SearchProductCategoryDTO ProductCategoryListObj { get; private set; }

        public ProductCategoryController(ProductCategoryServices ProductCategoryServices)
        {
            _ProductCategoryServices= ProductCategoryServices;
        }
        [HttpGet]
        public IActionResult CategoryList(SearchProductCategory obj)
        {
            SearchProductCategoryDTO obj1 = new SearchProductCategoryDTO();
            obj1.CategoryCode = obj.CategoryCode;
            obj1.CategoryName = obj.CategoryName;
            ViewBag.Category = _ProductCategoryServices.SearchProductCategory(obj1);
			return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int CategoryID)
        {
            CategoryEditDTO CategoryEditObj = new CategoryEditDTO();
            CategoryEditObj.ProductCategoryIdPk = CategoryID;
            ViewBag.ProductDetails = await _ProductCategoryServices.EditCategoryId(CategoryEditObj);
            EditProductCategoryModel CategoryEditViewModelObj = new EditProductCategoryModel();
            CategoryEditViewModelObj.ProductCategoryIdPk = CategoryEditObj.ProductCategoryIdPk;
            CategoryEditViewModelObj.CategoryCode = CategoryEditObj.CategoryCode;
            CategoryEditViewModelObj.CategoryName = CategoryEditObj.CategoryName;
            CategoryEditViewModelObj.Description = CategoryEditObj.Description;
            return View(CategoryEditViewModelObj);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(EditProductCategoryModel model)
        {
            CategoryEditDTO CategoryEditObj = new CategoryEditDTO();
            CategoryEditObj.ProductCategoryIdPk = model.ProductCategoryIdPk;
            CategoryEditObj.CategoryCode = model.CategoryCode;
            CategoryEditObj.CategoryName = model.CategoryName;
            CategoryEditObj.Description = model.Description;
            bool result = await _ProductCategoryServices.UpdateEditCategory(CategoryEditObj);
            ViewBag.Response = result;
            return View(model);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> AddCategory(ProductCategoryViewModel model)
        {
            ProductCategoryDTO obj = new ProductCategoryDTO();
            obj.CategoryCode = model.CategoryCode;  
            obj.CategoryName = model.CategoryName;  
            obj.Description = model.Description;
            obj.CreatedBy=UserName;
            bool result= await _ProductCategoryServices.AddCategory(obj);
            ViewBag.Response=result;
            return View(model);
        }
        public IActionResult ViewCategory()
        {
            return View();
        }

    }
}
