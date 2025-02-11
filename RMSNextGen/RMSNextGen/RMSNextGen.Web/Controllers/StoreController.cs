using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult StoreList()
        {
            //get all stores data and put it in a viewbag/viewdata/model
            return View();
        }

        [HttpPost]
        public IActionResult SearchStores()
        {
            //perform search operation
            //filter the list
            //
            return RedirectToAction("StoreList");
        }

        [HttpGet]
        public IActionResult EditStore()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewStore()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewStore()
        {
            return View();
        }

    }
}
