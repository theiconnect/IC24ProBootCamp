using System.ComponentModel.DataAnnotations;

namespace RMSNextGen.Web.Models
{
    public class StockViewModel
    {
		[Required(ErrorMessage = "StockCode is required")]
		[Display(Name = "StockCode")]
		public string StockCode { get; set; }

		[Required(ErrorMessage = "PurchaseOrderNumber is required")]
		[Display(Name = "PurchaseOrderNumber")]
		public string PurchaseOrderNumber { get; set; }

		[Required(ErrorMessage = "InvoiceNumber is required")]
		[Display(Name = "InvoiceNumber")]
		public string InvoiceNumber { get; set; }

		[Required(ErrorMessage = "StockInTime is required")]
		[Display(Name = "StockInTime")]
		public DateTime StockInTime { get; set; }

		[Required(ErrorMessage = "StockCode is required")]
		[Display(Name = "StockCode")]
		public string Remarks { get; set; }

		[Required(ErrorMessage = "VehicleNumber is required")]
		[Display(Name = "VehicleNumber")]
		public string VehicleNumber { get; set; }
		public string ApprovedBy { get; set; }
		public DateOnly ApprovedOn { get; set; }

		[Required(ErrorMessage = "ApprovedComments is required")]
		[Display(Name = "ApprovedComments")]
		public string ApprovedComments { get; set; }
		public string CreatedBy { get; set; }
		public DateOnly CreatedOn { get; set; }

		public string SupplierName { get; set; }
		public string StoreName { get; set; }
		public string ProductName { get; set; }

		public string StoreLocation {  get; set; }

		public DateTime Date {  get; set; }

	}
}
