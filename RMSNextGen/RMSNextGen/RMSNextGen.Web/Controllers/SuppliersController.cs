using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;
using Azure.Identity;
using RMSNextGen.DAL;
using Humanizer;

namespace RMSNextGen.Web.Controllers
{
	
    public class SuppliersController : Controller
    {
		string UserName = "Kiran";

		SupplierService _supplierService;
		private object _supplierRepository;

		public SuppliersController(SupplierService supplierService)
		{
			_supplierService = supplierService;
		}





		//SupplierEditService _supplierEditService;
		//private object _supplierEditRepository;

		//public SuppliersController(SupplierEditService supplierEditService)
		//{
		//	_supplierEditService = supplierEditService;
		//}
		[HttpGet]
		public IActionResult SupplierList()
        {
			//List<SupplierListDTO> SupplierListDTO = new List<SupplierListDTO>();
			//SupplierListDTO._supplierService.GetSupplierList();

			List<SupplierListDTO> supplierListDTO = _supplierService.GetSupplierList();

			return View(supplierListDTO);
        }
        [HttpGet]
		public IActionResult AddNewSupplier()
		{
            return View();
	}
	[HttpPost]
	public async Task<IActionResult> AddNewSupplier(SupplierViewModel model)
	{
		SupplierDTO DTO = new SupplierDTO();
		DTO.SupplierCode = model.SupplierCode;
		DTO.SupplierName = model.SupplierName;
		DTO.CompanyName = model.CompanyName;
		DTO.ContactNumber1 = model.ContactNumber1;
		DTO.ContactNumber2 = model.ContactNumber2;
		DTO.Email = model.Email;
		DTO.Address = model.Address;
		DTO.GSTNumber = model.GSTNumber;
		DTO.CreatedBy = UserName;
		DTO.CreatedOn = model.CreatedOn;
		bool result = await _supplierService.AddSupplier(DTO);

			ViewBag.Response = result;
			return View(model);
	}
	public IActionResult ViewSupplier()
        {
            return View();
        }

		//[HttpGet]
		//public IActionResult EditSupplier()
		//      {
		//          return View();
		//      }
		//[HttpPost]
		//public async Task<IActionResult> AddEditSupplier(SupplierEditViewModel model)
		//{
		//	SupplierEditDTO DTO = new SupplierEditDTO();
		//	DTO.SupplierName = model.SupplierName;
		//	DTO.CompanyName = model.CompanyName;
		//	DTO.ContactNumber1 = model.ContactNumber1;
		//	DTO.ContactNumber2 = model.ContactNumber2;
		//	DTO.Email = model.Email;
		//	DTO.Address = model.Address;
		//	DTO.GSTNumber = model.GSTNumber;
		//	DTO.CreatedBy = UserName;
		//	DTO.CreatedOn = model.CreatedOn;

		//   bool result = await _supplierEditService.EditSupplier(DTO);


		//	ViewBag.Message = result ? "Student Registered Successfully" : "Unalbe to register the student";
		//	//return RedirectToAction("SupplierList");
		//	return View(model);
		//}

		[HttpGet]
		public IActionResult SearchSupplier()
		{
			return View();
		}
	}
}
