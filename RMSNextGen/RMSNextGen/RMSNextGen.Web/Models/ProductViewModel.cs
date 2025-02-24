using System.ComponentModel.DataAnnotations;

namespace RMSNextGen.Web.Models
{
	public class ProductViewModel
	{
		[Required(ErrorMessage="ProductCode is required")]
		[Display(Name = "ProductCode")]
		public string ProductCode { get; set; }

		[Required(ErrorMessage ="ProductName is required")]
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		public string Category { get; set; }

		[Required(ErrorMessage = "PricePerUnit is required")]
		[Display(Name = "PricePerUnit")]
		public string PricePerUnit { get; set; }

		[Required(ErrorMessage = "ThresholdLimit is required")]
		[Display(Name = "ThresholdLimit")]
		public string ThresholdLimit { get; set; }
		public string UnitofMeasurement { get; set; }
		public string CreatedBy {  get; set; }
		public DateOnly CreatedOn { get; set; }


	}
}
