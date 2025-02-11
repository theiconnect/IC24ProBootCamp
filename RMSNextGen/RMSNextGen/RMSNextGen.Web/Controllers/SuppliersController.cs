using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class SuppliersController : Controller
    {
        public IActionResult SupplierList()
        {
            return View();
        }
        public IActionResult AddNewSupplier()
        {
            return View();
        }
        public IActionResult ViewSupplier()
        {
            return View();
        }
        public IActionResult EditSupplier()
        {
            return View();
        }
    }
}
