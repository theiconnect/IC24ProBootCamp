using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Mono.TextTemplating;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;




namespace RMSNextGen.Web.Controllers
{
	public class StoreController : RMSBaseController
    {
		StoreService _storeService;

		public StoreController(StoreService storeService)
		{
			_storeService = storeService;
		}


		[HttpGet]

		public IActionResult StoreList()
		{
			CityDTO cityobj=new CityDTO();
			//get all stores data and put it in a viewbag/viewdata/model
			var States = _storeService.GetStates();

            ViewBag.States = new SelectList(States, "StateId", "Name");

            ViewBag.StoreListData=_storeService.GetStores();

			//var Cities = _storeService.GetCities(0);

			//ViewBag.Cities = new SelectList(Cities, "CityID", "Name");

			return View();


		}
		//This will call when state dropdown value chagned or any state selected
		public List<CityDTO> GetCitiesByStateId(int SelectedStateId)
		{
			var cities = _storeService.GetCities(SelectedStateId);
			return cities;
		}
		[HttpPost]
		public IActionResult StoreList(IFormCollection form)
		{
			//perform search operation
			//filter the list
			//

			

            return RedirectToAction("StoreList", "Store");
		}


		[HttpGet]
		public IActionResult AddNewStore()
		{

            var States = _storeService.GetStates();

            ViewBag.States = new SelectList(States, "StateId", "Name");

            return View();
		}

		//[HttpPost]

		//public IActionResult SearchStores(StoreSearchViewModel model)

		//{
		//	SearchStoresDTO searchStores =new SearchStoresDTO();

		//	searchStores.StoreCode = model.StoreCode;
		//	searchStores.Location = model.Location;
		//	searchStores.City = model.City;
		//	searchStores.State = model.State;
		//	return View("StoreList", model);
		//}

		//[HttpPost]
		//      public IActionResult AddNewStore(IFormCollection form  )
		//      {
		//	return RedirectToAction("StoreList", "Store");
		//}

		[HttpPost]
		public async Task<IActionResult> AddNewStore(AddStoreViewModel model)
		{
			AddStoreDTO objectdto = new AddStoreDTO();

			objectdto.StoreCode = model.StoreCode;
			objectdto.StoreLocation = model.StoreLocation;
			objectdto.StoreLocation = model.StoreLocation;
			objectdto.FAX = model.FAX;
			objectdto.IsCorporateOffice = model.IsCorporateOffice;
			objectdto.NickName = model.NickName;
			objectdto.Address = model.Address;
			objectdto.OfficeNo = model.OfficeNo;
			objectdto.ManagerName = model.ManagerName;
			objectdto.ManagerNo = model.ManagerNo;
			objectdto.GSTNo = model.GSTNo;
			objectdto.CINNo = model.CINNo;
			objectdto.StoreName = model.StoreName;
			objectdto.ContactNumber = model.ContactNumber;

			objectdto.StoreLocation = model.StoreLocation;
			objectdto.CreatedBy = UserName;
			bool Result=await _storeService.AddStore(objectdto);
								
			ViewBag.message = Result ? "Hey Good  ur AddStore Job Completed Succesfully" : "Unable To  Add The Store";

			return View(model);

		}
		[HttpGet]
		public IActionResult GetStore()
		{
			return View();
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
