using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult ProductList()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductList(IFormCollection form)
        {
            
            return RedirectToAction("ProductList","Product");
        }
        [HttpGet]
        public IActionResult AddNewProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewProduct(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");

            
        }
        [HttpGet]
        public IActionResult EditProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditProduct(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


        }
        [HttpGet]
        public IActionResult ViewProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Save(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


        }
        [HttpPost]
        public IActionResult Search(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


        }
    }
}
