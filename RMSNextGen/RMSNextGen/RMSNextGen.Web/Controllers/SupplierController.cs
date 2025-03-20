using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class SupplierController : RMSBaseController
    {
        [HttpGet]
        public IActionResult SupplierList()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SupplierList(IFormCollection Form)
        {
            return RedirectToAction("SupplierList", "Supplier");
        }
        [HttpPost]
        public IActionResult AddNewSupplier(IFormCollection Form)
        {
            return RedirectToAction("SupplierList", "Supplier");
        }
        [HttpPost]
        public IActionResult EditSupplier(IFormCollection Form) 
        {
            return RedirectToAction("SupplierList", "Supplier");
        }
        [HttpGet]
        public IActionResult AddNewSupplier()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewSupplier()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditSupplier()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Save()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Back()
        {
            return View();
        }
    }
}
