using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class ProductDTO
	{
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string Category { get; set; }
		public decimal PricePerUnit { get; set; }
		public decimal ThresholdLimit { get; set; }
		public string UnitofMeasurement { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedOn {  get; set; }

	}
}
