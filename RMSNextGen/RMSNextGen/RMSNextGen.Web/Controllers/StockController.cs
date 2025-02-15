using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
	public class StockController : Controller
	{
		[HttpGet]
		public IActionResult StockList()
		{
			return View();
		}
		[HttpGet]
		public IActionResult AddStock()
		{
			return View();
		}
        [HttpGet]
        public IActionResult ViewStock()
        {
            return View();
        }
    }
}
