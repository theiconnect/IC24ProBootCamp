using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
	public class ProductServices
	{
		ProductRepository _productRepository;
		public ProductServices(ProductRepository productRepository) 
		{
			_productRepository = productRepository;
		}
		public async Task<bool> SaveProduct(ProductDTO productObj)
		{
			return await _productRepository.SaveProduct(productObj);

		}
		
		public List<ProductListDTO> GetProducts(ProductSearchDTO searchObj)
		{
			return _productRepository.GetProducts(searchObj);

		}
	}
}
