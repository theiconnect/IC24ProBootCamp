using KrishnaveniCR.Model;
using KrishnaveniCR.Services;
using Microsoft.AspNetCore.Mvc;

namespace KrishnaveniCR.web.Controllers
{
	public class CompanyController : Controller
	{
		private CompanyServices _companyService;
		public CompanyController(CompanyServices companyService)
		{
			_companyService = companyService;
		}
		[HttpGet]
		public IActionResult Registration()
		{
			CompanyDTO company = _companyService.GetCompanyDetails();
			return View(company);

		}
		public IActionResult Registration1()
		{
			CompanyDTO company = _companyService.GetCompanyDetails();
			return View(company);

		}
		public IActionResult HRDetails()
		{
			CompanyDTO company = _companyService.GetCompanyDetails();
			return View(company);

		}
	}
}
