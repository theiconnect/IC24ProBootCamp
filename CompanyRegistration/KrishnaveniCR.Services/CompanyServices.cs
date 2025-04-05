using KrishnaveniCR.Model;
using KrishnaveniCR.Repositry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaveniCR.Services
{
	public class CompanyServices
	{
		private CompanyRepositry _companyRepository;
		public CompanyServices(CompanyRepositry companyRepository)
		{
			_companyRepository = companyRepository;
		}
		public CompanyDTO GetCompanyDetails()
		{
			return _companyRepository.GetCompanyDetails();
		}

	}
}
