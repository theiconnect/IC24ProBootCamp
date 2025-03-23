using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
    public class ProductCategoryDTO
    {
		public int ProductCategoryId { get; set; }

		public string ProductCategoryCode { get; set; }
		public string ProductCategoryName { get; set; }

		public string Description { get; set; }

		public string CreatedBy { get; set; }


	}
}
