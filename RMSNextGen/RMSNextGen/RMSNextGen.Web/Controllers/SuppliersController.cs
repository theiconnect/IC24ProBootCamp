using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;

namespace RMSNextGen.Web.Controllers
{
    public class SuppliersController : Controller
    {
		SupplierService _supplierService;
		public SuppliersController(SupplierService supplierService)
		{
			_supplierService = supplierService;
		}
		public IActionResult SupplierList()
        {
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
			DTO.FirstName = model.FirstName;
			DTO.LastName = model.LastName;
			DTO.CompanyName = model.CompanyName;
			DTO.MobileNumber = model.MobileNumber;
			DTO.Address = model.Address;
			DTO.City = model.City;
			DTO.State = model.State;
			DTO.FaxNumber = model.FaxNumber;
			DTO.GstNo = model.GstNo;

			return View(model);
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
