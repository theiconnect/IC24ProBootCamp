using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMSNextGen.DAL;
using RMSNextGen.Models;

namespace RMSNextGen.Services
{
    public class ProductCategoryServices
    {
        ProductCategoryRepository _categoryRepository;
        public ProductCategoryServices(ProductCategoryRepository ProductCategoryRepository)
        {
            _categoryRepository = ProductCategoryRepository;

        }
        public async Task<bool> AddCategory(ProductCategoryDTO category)
        {
            return await _categoryRepository.AddCategory(category);

        }
        public  List<ProductCategoryListDTO> GetProductCategoryList()
        {
            return  _categoryRepository.GetProductCategoryList();
        }
        public List<ProductCategoryListDTO> SearchProductCategory(SearchProductCategoryDTO ProductCategoryListObj)
        {
            return _categoryRepository.SearchProductCategory(ProductCategoryListObj);

        }
    }
}
