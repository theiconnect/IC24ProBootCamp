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
        public ProductCategoryController(ProductCategoryServices ProductCategoryServices)
        {
            _ProductCategoryServices= ProductCategoryServices;
        }
        [HttpGet]
        public IActionResult CategoryList()
        {
           ViewBag.Category = _ProductCategoryServices.GetProductCategoryList();
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
