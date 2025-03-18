using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public  class AddStoreDTO
	{		
		    public string StoreCode { get; set; }

			public string StoreLocation { get; set; }

			public string NickName { get; set; }

			public string Address { get; set; }

		     public string State { get; set; }

		     public string City { get; set; }

			public string OfficeNo { get; set; }

			public string ManagerName { get; set; }

			public string ManagerNo { get; set; }

			public string GSTNo { get; set; }

			public string CINNo { get; set; }

		    public string FAX { get; set; } 
		
		    public string IsCorporateOffice { get; set; }

		     public string StoreName { get; set; }
	
			  public string CreatedBy { get; set; }

		    public DateTime CreatedOn { get; set; }

		    public string ContactNumber { get; set; }

	}
}
