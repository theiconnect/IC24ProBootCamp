using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
    public class ProductEditDTO
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
