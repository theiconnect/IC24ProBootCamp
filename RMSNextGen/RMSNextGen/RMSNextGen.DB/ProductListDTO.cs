using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class ProductListDTO
	{
		public int ProductID { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string Category { get; set; }

		public string PricePerUnit { get; set; }
		public decimal Quantity {  get; set; }
		
	}
}
