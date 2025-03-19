using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;
using Azure.Identity;
using RMSNextGen.DAL;
using Humanizer;
using System.Reflection;

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

			SearchSupplierDTO searchDTO = new SearchSupplierDTO();
			ViewBag.Supplier =  _supplierService.GetSupplierList(searchDTO);

			//List<SupplierListDTO> supplierListDTO = _supplierService.GetSupplierList(searchDTO);

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SupplierList(SearchSupplierViewModel model)
		{
			SearchSupplierDTO searchDTO = new SearchSupplierDTO();
			searchDTO.SupplierName = model.SupplierName;
			searchDTO.CompanyName = model.CompanyName;
			searchDTO.Address = model.Address;
			ViewBag.Supplier =   _supplierService.GetSupplierList(searchDTO);

			return View();

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

		[HttpGet]
		public IActionResult EditSupplier(int SupplierID)
		{
			SupplierEditDTO DTO = new SupplierEditDTO();
			DTO.SupplierIdPk = Convert.ToInt32(SupplierID);
			ViewBag.Supplier = _supplierService.EditSupplierDetails(DTO);
			//map DTO to view model
			SupplierEditViewModel viewModel = new SupplierEditViewModel();
				viewModel.SupplierIdPk = DTO.SupplierIdPk;
			viewModel.SupplierName = DTO.SupplierName;
				viewModel.CompanyName = DTO.CompanyName;
			    viewModel.Address = DTO.Address;
			return View(viewModel);
		}
		[HttpPost]
		public async  Task<IActionResult> EditSupplier(SupplierEditViewModel viewModel)
		{
			SupplierEditDTO DTO = new SupplierEditDTO();
			DTO.SupplierIdPk = viewModel.SupplierIdPk;
			DTO.SupplierName = viewModel.SupplierName;
			DTO.CompanyName = viewModel.CompanyName;
			DTO.Address = viewModel.Address;
			bool result = await  _supplierService.UpdateSupplierDetails(DTO);
			ViewBag.Response = result;
			 return View(viewModel);
		}

	}
	
	
}
