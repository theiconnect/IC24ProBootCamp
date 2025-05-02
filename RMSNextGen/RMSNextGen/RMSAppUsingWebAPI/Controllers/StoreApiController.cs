using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMSAppUsingWebAPI.DAL;
using RMSNextGen.Models;

namespace RMSAppUsingWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StoreApiController : ControllerBase
	{
		private readonly StoreApiRepository _storeApiRepository;

		public StoreApiController(StoreApiRepository StoreApiRepository)
		{
			_storeApiRepository = StoreApiRepository;
		}

		[HttpGet("StoreList")]
		//[Authorize(Roles = "Employee")]
		// Only require authentication for this method
		//Checking token in Authorization Bearer space token with out double codes 
		public IActionResult StoreList()
		{
			var stores=_storeApiRepository.GetStores(new SearchStoresDTO());//empty search DTO
			return Ok(stores);

		}
		 // This ensures that only authenticated users can access this API

		[HttpGet("SearchStores")]
		[ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "storeCode", "storeLocation", "city", "state" })]
		[Authorize(Roles = "Admin,Employee")]
		//[Authorize]


		public IActionResult SearchStores(
		string? storeCode = null,
		string? storeLocation = null,
		string? city = null,
		string? state = null)
        {
            var searchDto = new SearchStoresDTO
            {
                StoreCode = storeCode,
                Location = storeLocation,
                City = city,
                State = state
            };

            var stores = _storeApiRepository.GetStores(searchDto);
            return Ok(stores);
        }

		//[Authorize(Roles = "Employee")]
		// This ensures that only authenticated users can access this API

		// GET: api/StoreApi/AddNewStore
		[Authorize]
		[HttpGet("AddNewStore")]
		public IActionResult AddNewStore()
		{
			return Ok("Use POST to add a new store");

		}
		[Authorize(Roles = "Admin,Employee")]
		// This ensures that only authenticated users can access this API

		// POST: api/StoreApi/AddNewStore
		[HttpPost("AddNewStore")]
		public async Task<IActionResult> AddNewStore([FromBody] AddStoreDTO store)
		{
			if (store == null)
			{
				return BadRequest("Store data is required.");
			}

			bool isAdded = await _storeApiRepository.AddStoreAsync(store);

			if (isAdded)
			{
				return Ok("Store added successfully.");
			}
			else
			{
				return StatusCode(500, "Error occurred while adding store.");
			}
		}
		[Authorize(Roles = "Admin,Manager")]
		// This ensures that only authenticated users can access this API

		// GET: api/StoreApi/GetStoreById/5
		[HttpGet("GetStoreById/{storeIdPk}")]
        public async Task<IActionResult> GetStoreById(int storeIdPk)
        {
            var store = await _storeApiRepository.GetStoreByIdAsync(storeIdPk);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            return Ok(store);
        }

		[Authorize(Roles = "Admin,Manager")]
		// This ensures that only authenticated users can access this API

		// POST: api/StoreApi/UpdateStore
		[HttpPost("UpdateStore")]
		public async Task<IActionResult> EditStore([FromBody] StoreEditDTO storeEditObj)
		{
			if (storeEditObj == null)
			{
				return BadRequest("Invalid store data.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Returns 400 Bad Request with validation errors
			}

			try
			{
				bool isUpdated = await _storeApiRepository.UpdateStoreAsync(storeEditObj);

				if (isUpdated)
				{
					return Ok("Store updated successfully.");
				}
				else
				{
					return NotFound("Store not found or not updated.");
				}
			}
			catch (Exception ex)
			{
				// Optional: log the exception if needed
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

	}
}
