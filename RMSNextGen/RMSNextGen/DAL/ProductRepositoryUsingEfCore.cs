using RMSNextGen.Models.ModelsUsingEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore; // For DbContext




namespace RMSNextGen.DAL
{
	public class ProductRepositoryUsingEfCore
	{
		private readonly RmsnextGenContext _context;
		public ProductRepositoryUsingEfCore(RmsnextGenContext context)
		{
			_context = context;
		}
		public List<ProductMaster> GetProducts()
		{
			return _context.ProductMasters.ToList();
		}
		public bool RemoveProduct(int ProductId)
		{
			var product= _context.ProductMasters.FirstOrDefault(p=>p.ProductIdPk==ProductId);
			if (product != null) 
			{
				_context.ProductMasters.Remove(product);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
