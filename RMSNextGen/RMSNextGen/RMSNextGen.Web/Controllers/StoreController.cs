using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class StoreController : Controller
    {
        [HttpGet]
        public IActionResult StoreList()
        {
            //get all stores data and put it in a viewbag/viewdata/model
            return View();

		}
		[HttpPost]
        public IActionResult StoreList(IFormCollection form)
        {
            //perform search operation
            //filter the list
            //
            return RedirectToAction("StoreList","Store");
        }
               

		[HttpGet]
		public IActionResult AddNewStore()
		{
            return View();
		}

		[HttpPost]
        public IActionResult AddNewStore(IFormCollection form  )
        {
			return RedirectToAction("StoreList", "Store");
		}
		[HttpGet]
		public IActionResult EditStore()
		{
			return View();
		}

		[HttpPost]
		public IActionResult EditStore(IFormCollection form)
		{
			return RedirectToAction("StoreList", "Store");
		}


		[HttpGet]
        public IActionResult ViewStore()
        {
            return View();
        }

    }
}
