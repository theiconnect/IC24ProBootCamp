using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductList()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchProducts()
        {
            //perform search operation
            //filter the list
            //
            return RedirectToAction("ProductList");
        }
        [HttpGet]
        public IActionResult AddNewProduct()
        {
            


			return View();
        }
        public IActionResult EditProduct()
        {
            return View();
        } 
        public IActionResult ViewProduct()
        {
            return View();
        }
    }
}
