using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class StockDTO
	{
		public string StockCode { get; set; }
		public string PurchaseOrderNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime StockInTime { get; set; }
		public string Remarks { get; set; }
		public string VehicleNumber { get; set; }
		public string ApprovedBy { get; set; }
		public DateOnly ApprovedOn { get; set; }
		public string ApprovedComments { get; set; }
		public string CreatedBy { get; set; }
		public DateOnly CreatedOn { get; set; }


		public string SupplierName { get; set; }
		public string StoreName { get; set; }
		public string ProductName { get; set; }

		public string StoreLocation { get; set; }

	}
}
