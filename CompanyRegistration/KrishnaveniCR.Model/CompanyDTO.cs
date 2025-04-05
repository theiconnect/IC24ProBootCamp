using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaveniCR.Model
{
	public class CompanyDTO
	{

		public int CompanyIdPk { get; set; }
		public string CompanyName { get; set; }
		public string RegistrationNumber { get; set; }
		public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
		public List<HRDetailDto> HRDetails { get; set; } = new List<HRDetailDto>();
	}

	public class AddressDto
	{
		public int CompanyAddressIdPk { get; set; }
		public string AddressType { get; set; }
		public string HouseNo { get; set; }
		public string Street { get; set; }
		public string LandMark { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}

	public class HRDetailDto
	{
		public int HRDetailIdPk { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string UserName { get; set; }
		public string Gender { get; set; }
		public DateTime DOB { get; set; }
		public string Email { get; set; }

	}
}
