using RMSNextGen.DAL;
using RMSNextGen.Models;
using RMSNextGen.Models.ModelsUsingEFCore;
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
		ProductRepositoryUsingEfCore _productRepositoryUsingEfCore;
		public ProductServices(ProductRepository productRepository, ProductRepositoryUsingEfCore productRepositoryUsingEfCore) 
		{
			_productRepository = productRepository;
			_productRepositoryUsingEfCore = productRepositoryUsingEfCore;
		}
		public async Task<bool> SaveProduct(ProductDTO productObj)
		{
			return await _productRepository.SaveProduct(productObj);

		}
		
		//public List<ProductListDTO> GetProducts(ProductSearchDTO searchObj)
		//{
		//	return _productRepository.GetProducts(searchObj);

		//}
		public List<ProductMaster> GetProductsUsingEFCore()
		{
			return _productRepositoryUsingEfCore.GetProducts();
		}
		public async Task<bool> GetProductCode()
		{
			return await _productRepository.GetProductCode();
		}
		public List<ProductCategoryDTO> GetProductCategory()
		{
			return _productRepository.GetProductCategory();
		}
		public List<ProductUTMDTO> GetUTM()
		{
			return _productRepository.GetUTM();
		}
		public async Task<bool> GetProductBasedOnId(ProductEditDTO productEditObj)
		{
			return await _productRepository.GetProductBasedOnId(productEditObj);

		}
		public async Task<bool> UpdateProducts(ProductEditDTO productEditObj)
		{
			return await _productRepository.UpdateProducts(productEditObj);
		}
	}
}
