using System.ComponentModel.DataAnnotations;

namespace RMSNextGen.Web.Models
{
	public class ProductViewModel
	{
		[Required(ErrorMessage="ProductCode is required")]
		[Display(Name = "ProductCode")]
		public string ProductCode { get; set; }

		[Required(ErrorMessage ="ProductName is required")]
		[MaxLength(500, ErrorMessage ="ProductName cannot exceed 500 characters")]
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }

		//[Required(ErrorMessage = "Category is required")]
		//[Display(Name = "Category")]
		public string Category { get; set; }

		[Required(ErrorMessage = "PricePerUnit is required")]
		//double is a type, while double.MaxValue is an actual number.
		[Range(0.01, double.MaxValue, ErrorMessage ="Please enter a valid price greater than Zero")]
		[Display(Name = "PricePerUnit")]
		public decimal PricePerUnit { get; set; }
		public decimal ThresholdLimit { get; set; }

		//[Required(ErrorMessage = "UnitofMeasurement is required")]
		//[Display(Name = "UnitofMeasurement")]
		public string UnitofMeasurement { get; set; }
		public string CreatedBy {  get; set; }
		public DateTime CreatedOn { get; set; }


	}
}
