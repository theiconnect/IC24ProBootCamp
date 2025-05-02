using Microsoft.AspNetCore.Authorization;
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
        string userName = "Krishnaveni";
        StoreService _storeService;
        

        public StoreController(StoreService storeService)
		{
			_storeService = storeService;
		}
		[Authorize(Roles = "Admin,Manager")]
		[HttpGet]
		public async Task<IActionResult> StoreList()
		{
			ViewBag.StoreListData = await _storeService.GetStoresFromApiAsync();

			
			return View();
		}
		//[Authorize(Roles = "user")]
		[Authorize(Roles = "Admin,Manager")]

		[HttpPost]
        [Route("SearchStore")]
        public async Task<IActionResult> SearchStore(StoreSearchViewModel model)
        {
            SearchStoresDTO searchDto = new SearchStoresDTO();
            searchDto.StoreCode=model.StoreCode;
            searchDto.Location = model.Location;
            searchDto.City = model.City;
            searchDto.State = model.State;

			ViewBag.StoreListData = await  _storeService.SearchStoresAsync(searchDto);

            

            return View("StoreList");


        }
		[Authorize(Roles = "Admin,Employee")]
		[HttpGet]
		public async Task<IActionResult> AddNewStore()
		{
			return View();
		}
		[Authorize(Roles = "Admin,Employee")]
		[HttpPost]
        public async Task<IActionResult> AddNewStore(AddStoreViewModel model)
        {
			 // This fetches the logged-in username


			 // or get username from session/login

			if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                ViewBag.Message = "Model is not valid!";
                return View(model);
            }


            AddStoreDTO store = new AddStoreDTO
            {
                StoreCode = model.StoreCode,
                StoreLocation = model.StoreLocation,
                FAX = model.FAX,
                IsCorporateOffice = model.IsCorporateOffice,
                OfficeNo = model.OfficeNo,
                ManagerName = model.ManagerName,
                ManagerNo = model.ManagerNo,
                GSTNo = model.GSTNo,
                CINNo = model.CINNo,
                StoreName = model.StoreName,
                ContactNumber = model.ContactNumber,
                City=model.City,
                State=model.State,
                Address=model.Address,
                NickName=model.NickName,
                CreatedBy = model.CreatedBy, // Make sure userName is fetched from claims or session
                CreatedOn = DateTime.Now

            };

            try
            {
                bool result = await _storeService.AddStoreAsync(store);

                if (result)
                {
                    TempData["Message"] = "Store added successfully!";
                    return RedirectToAction("StoreList", "Store");
                }
                else
                {
                    ViewBag.Message = "Error occurred while adding store.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Optional: log exception
                ViewBag.Message = $"Exception: {ex.Message}";
                return View(model);
            }
        }

		[Authorize(Roles = "Admin,Manager")]
		[HttpGet]
		public async Task<IActionResult> EditStore(int storeId)
		{
			// Get full store details from API
			var storeEditObj = await _storeService.GetStoreByIdAsync(storeId);

			if (storeEditObj == null)
			{
				return NotFound("Store not found");
			}

			// Map the data to ViewModel
			StoreEditViewModel storeEditViewModelObj = new StoreEditViewModel
			{
				StoreId = storeEditObj.StoreId,
				StoreCode = storeEditObj.StoreCode,
				StoreLocation = storeEditObj.StoreLocation,
				ManagerName = storeEditObj.ManagerName,
				ManagerNo = storeEditObj.ManagerNo,
				GSTNo = storeEditObj.GSTNo,
				CINNo = storeEditObj.CINNo
			};

			return View(storeEditViewModelObj);
		}
		

		
		[Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> EditStore(StoreEditViewModel model)
        {
			if (!ModelState.IsValid)
			{
				// Return the view with validation messages
				return View(model);
			}
			// Create a DTO from the view model object
			StoreEditDTO storeEditObj = new StoreEditDTO
            {
                StoreId = model.StoreId,  // StoreId for identifying the store
                StoreCode = model.StoreCode,
                StoreLocation = model.StoreLocation,
                NickName=model.NickName,
                Address=model.Address,
                OfficeNo=model.OfficeNo,
                ManagerName = model.ManagerName,
                ManagerNo = model.ManagerNo,
                GSTNo = model.GSTNo,
                CINNo = model.CINNo
            };

            // Call the service to update the store details
            bool result = await _storeService.UpdateStoreAsync(storeEditObj);

            // Provide feedback based on the result
            ViewBag.Response = result;

            // Return the same View with the updated data
            return View(model);
        }

        //[HttpGet]

        //public IActionResult StoreList()
        //{
        //	CityDTO cityobj=new CityDTO();
        //	//get all stores data and put it in a viewbag/viewdata/model
        //	var States = _storeService.GetStates();

        //          ViewBag.States = new SelectList(States, "StateId", "Name");

        //          ViewBag.StoreListData=_storeService.GetStores();

        //	//var Cities = _storeService.GetCities(0);

        //	//ViewBag.Cities = new SelectList(Cities, "CityID", "Name");

        //	return View();


        //}
        ////This will call when state dropdown value chagned or any state selected
        //public List<CityDTO> GetCitiesByStateId(int SelectedStateId)
        //{
        //	var cities = _storeService.GetCities(SelectedStateId);
        //	return cities;
        //}
        //[HttpPost]
        //public IActionResult StoreList(IFormCollection form)
        //{
        //	//perform search operation
        //	//filter the list
        //	//



        //          return RedirectToAction("StoreList", "Store");
        //}

        //[Authorize]
        //[HttpGet]
        //public IActionResult AddNewStore()
        //{

        //          var States = _storeService.GetStates();

        //          ViewBag.States = new SelectList(States, "StateId", "Name");

        //          return View();
        //}

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

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> AddNewStore(AddStoreViewModel model)
        //{
        //	AddStoreDTO objectdto = new AddStoreDTO();

        //	objectdto.StoreCode = model.StoreCode;
        //	objectdto.StoreLocation = model.StoreLocation;

        //	objectdto.FAX = model.FAX;
        //	objectdto.IsCorporateOffice = model.IsCorporateOffice;

        //	objectdto.OfficeNo = model.OfficeNo;
        //	objectdto.ManagerName = model.ManagerName;
        //	objectdto.ManagerNo = model.ManagerNo;
        //	objectdto.GSTNo = model.GSTNo;
        //	objectdto.CINNo = model.CINNo;
        //	objectdto.StoreName = model.StoreName;
        //	objectdto.ContactNumber = model.ContactNumber;

        //	objectdto.StoreLocation = model.StoreLocation;
        //	objectdto.CreatedBy = UserName;
        //	//bool Result=await _storeService.AddStore(objectdto);

        //	//ViewBag.message = Result ? "Hey Good  ur AddStore Job Completed Succesfully" : "Unable To  Add The Store";

        //	return View(model);

        //}
        //      [HttpGet]
        //public IActionResult GetStore()
        //{
        //	return View();
        //}




        [Authorize]
		[HttpGet]
		public IActionResult ViewStore()
		{
			return View();
		}

	}
}
