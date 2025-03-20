using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RMSNextGen.Web.Controllers
{
    public class ProductController : RMSBaseController
    {
        string userName = "Krishnaveni";
        ProductServices _productServices;
        public ProductController(ProductServices productServices)
        {
			_productServices = productServices;
        }

		[HttpGet]
        public  IActionResult ProductList()
        {
			ProductSearchDTO searchObj = new ProductSearchDTO();
			ViewBag.Product = _productServices.GetProducts(searchObj);
			



			return View();
        }
		[HttpPost]
		public async Task<IActionResult> ProductList(ProductSearchViewModel productSearchObj)
		{

            
			ProductSearchDTO searchObj = new ProductSearchDTO();
			searchObj.ProductCode = productSearchObj.ProductCode;
			searchObj.ProductName = productSearchObj.ProductName;

			ViewBag.Product =  _productServices.GetProducts(searchObj);
            return View();
        }
		//[HttpPost]
		//public async Task<IActionResult> SearchProduct(ProductSearchViewModel productSearchObj)
		//{


		//	ProductSearchDTO searchObj = new ProductSearchDTO();
		//	searchObj.ProductCode = productSearchObj.ProductCode;
		//	searchObj.ProductName = productSearchObj.ProductName;

		//	ViewBag.Product = _productServices.GetProducts(searchObj);
		//	return View("ProductList");
		//}


		[HttpGet]
        public IActionResult AddNewProduct()
        {
			//ViewBag.ProductCode = _productServices.GetProductCode();
			//string productCode = Convert.ToString(ViewBag.ProductCode);

			//if (String.IsNullOrEmpty(productCode))
			//{
			//    ViewBag.ProductCode = "P-001";

			//}
			//else
			//{
			//    string[] productCodeArray = productCode.Split('-');

			//    productCodeArray[1] = (productCodeArray[1]) + 1;

			//    productCode = productCodeArray[0] + productCodeArray[1];

			//}
			var productCategories = _productServices.GetProductCategory();
			var productUOM = _productServices.GetUTM();
			ViewBag.ProductCategory = new SelectList(productCategories, "ProductCategoryId", "ProductCategoryName");
			ViewBag.ProductUOM = new SelectList(productUOM, "UOMIdPk", "UOMName");
			//SelectList is a class and helps you easily bind a collection to a dropdown list in your view.SelectList(collection,value,text)

			return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewProduct(ProductViewModel model)
        {
			//return RedirectToAction("ProductList", "Product");
			ProductDTO productObj=new ProductDTO();
            productObj.ProductName = model.ProductName;
            productObj.ProductCode= model.ProductCode;
            
            productObj.PricePerUnit= model.PricePerUnit;
            productObj.ThresholdLimit=model.ThresholdLimit;
            
            productObj.CreatedBy = userName;
            productObj.CreatedOn = model.CreatedOn;
            
			bool result = await _productServices.SaveProduct(productObj);


            ViewBag.Response = result;
			return View(model);
		}
  //      public List<ProductUTMDTO> GetUTMByProductCategory(int selectedCategoryId)
  //      {
  //          var UTM = _productServices.GetProductCategory(selectedCategoryId);

  //          return UTM;


		//}

		//ViewBag.Message = result ? "Product Added Successfully" : "Unable to Add Product";

		//         try
		//         {
		//             if (ModelState.IsValid)
		//             {
		//                 bool result = await _productServices.SaveProduct(productObj);

		//                 ViewBag.Response = result;
		//                 if (result)
		//                 {
		//                     return RedirectToAction("AddNewProduct", "Product");

		//                 }
		//                 else
		//                 {
		//                     ModelState.AddModelError("", "Invalid Product Details.");
		//                     return View(model);
		//                 }



		//             }



		//         }
		//         catch (Exception ex) 
		//         {
		//             throw;
		//         }
		//ViewBag.Message = "Product Details Not Saved Successfully";






		[HttpGet]
        public async Task<IActionResult> EditProduct(int ProductId)
        {
			ProductEditDTO productEditObj = new ProductEditDTO();
			productEditObj.ProductIdPk = ProductId;
			ViewBag.ProductDetails = await _productServices.GetProductBasedOnId(productEditObj);
			ProductEditViewModel productEditViewModelObj=new ProductEditViewModel();
			productEditViewModelObj.ProductIdPk= productEditObj.ProductIdPk;
			productEditViewModelObj.ProductName= productEditObj.ProductName;
			productEditViewModelObj.ProductCode = productEditObj.ProductCode;
			productEditViewModelObj.ThresholdLimit= productEditObj.ThresholdLimit;
			productEditViewModelObj.PricePerUnit= productEditObj.PricePerUnit;
			productEditViewModelObj.Category = productEditObj.Category;
			productEditViewModelObj.UnitofMeasurement = productEditObj.UnitofMeasurement;

			return View(productEditViewModelObj);
        }
		[HttpPost]
		public async Task<IActionResult> EditProduct(ProductEditViewModel productEditViewModelObj)
		{
			ProductEditDTO productEditObj = new ProductEditDTO(); productEditObj.ProductName = productEditViewModelObj.ProductName;
			productEditObj.ProductCode = productEditViewModelObj.ProductCode;

			productEditObj.PricePerUnit = productEditViewModelObj.PricePerUnit;
			productEditObj.ThresholdLimit = productEditViewModelObj.ThresholdLimit;

			

			bool result = await _productServices.UpdateProducts(productEditObj);


			ViewBag.Response = result;
			return View(productEditViewModelObj);


		}
		[HttpGet]
        public IActionResult ViewProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Save(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


        }
        
	}
}
