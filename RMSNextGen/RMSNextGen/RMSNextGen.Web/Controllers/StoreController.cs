using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Web.Models;
using System.Reflection;

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

		[HttpPost]
		public IActionResult AddNewStore(AddNewStoreViewModel model)
		{
			AddNewStoreDTO objectdto = new AddNewStoreDTO();

			objectdto.StoreCode = model.StoreCode;
			objectdto.StoreLocation = model.StoreLocation;
			objectdto.NickName = model.NickName;
			objectdto.Address = model.Address;
			objectdto.OfficeNo = model.OfficeNo;
			objectdto.ManagerName = model.ManagerName;
			objectdto.ManagerNo = model.ManagerNo;
			objectdto.GSTNo = model.GSTNo;
			objectdto.CINNo = model.CINNo;
			objectdto.StoreLocation = model.StoreLocation;

			return View(model);
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
