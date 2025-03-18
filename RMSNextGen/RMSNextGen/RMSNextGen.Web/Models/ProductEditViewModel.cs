namespace RMSNextGen.Web.Models
{
	public class ProductEditViewModel
	{
		public int ProductIdPk { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string Category { get; set; }
		public decimal PricePerUnit { get; set; }
		public decimal ThresholdLimit { get; set; }
		public string UnitofMeasurement { get; set; }


	}
}
