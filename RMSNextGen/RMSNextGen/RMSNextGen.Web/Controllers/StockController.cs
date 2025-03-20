using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Models;
using RMSNextGen.Services;
using RMSNextGen.Web.Models;

namespace RMSNextGen.Web.Controllers
{
	
	public class StockController : RMSBaseController
    {
		string userName = "krishnaveni";

		StockServices _stockServices;
		public StockController(StockServices stockServices) 
		{ 
			_stockServices = stockServices;
		}
		[HttpGet]
		public IActionResult StockList()
		{
			return View();
		}
		[HttpGet]
		public IActionResult AddNewStock()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddNewStock(StockViewModel model)
		{
			StockDTO stockObj= new StockDTO();
			stockObj.StockCode = model.StockCode;
			stockObj.InvoiceNumber = model.InvoiceNumber;
			stockObj.PurchaseOrderNumber = model.PurchaseOrderNumber;
			stockObj.VehicleNumber = model.VehicleNumber;
			stockObj.StockInTime = model.StockInTime;
			stockObj.Remarks = model.Remarks;
			stockObj.ApprovedBy = userName;
			stockObj.ApprovedOn = model.ApprovedOn;
			stockObj.ApprovedComments = model.ApprovedComments;
			stockObj.CreatedBy = userName;
			stockObj.CreatedOn = model.CreatedOn;

			bool result=await _stockServices.SaveStock(stockObj);

			//ViewBag.Message = result ? "Stock Added Successfully" : "Unable to Add Stock";
			ViewBag.Response = result;

			return View(model);
		}
	}
}
