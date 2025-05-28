using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; // Add this namespace for logging



namespace RMSNextGen.Web.Controllers
{
	

	public class ProductController : RMSBaseController
    {
		private readonly ILogger<ProductController> _logger;  // Declare the logger

		string userName = "Krishnaveni";
        ProductServices _productServices;
		
		public ProductController(ProductServices productServices, ILogger<ProductController> logger)
        {
			_productServices = productServices;
			_logger = logger;  // Assign logger



		}


		[HttpGet]
        public  IActionResult ProductList()
        {
			_logger.LogInformation("Accessed Product List page.");

			ProductSearchDTO searchObj = new ProductSearchDTO();
			//ViewBag.Product = _productServices.GetProducts(searchObj);
			ViewBag.Product = _productServices.GetProductsUsingEFCore();


			var productCategories = _productServices.GetProductCategory();
			ViewBag.ProductCategory = new SelectList(productCategories, "ProductCategoryId", "ProductCategoryName");




			return View();
        }
		//[HttpPost]
		//public async Task<IActionResult> ProductList(ProductSearchViewModel productSearchObj)
		//{


		//	ProductSearchDTO searchObj = new ProductSearchDTO();
		//	searchObj.ProductCode = productSearchObj.ProductCode;
		//	searchObj.ProductName = productSearchObj.ProductName;

		//	ViewBag.Product =  _productServices.GetProducts(searchObj);
		//          return View();
		//      }
		[HttpPost]
        [Route("SearchProduct")]
        public async Task<IActionResult> SearchProduct(ProductSearchViewModel productSearchObj)
		{
			_logger.LogInformation("Searching products with ProductCode: {ProductCode} and ProductName: {ProductName}",
	productSearchObj.ProductCode, productSearchObj.ProductName);




			ProductSearchDTO searchObj = new ProductSearchDTO();
			searchObj.ProductCode = productSearchObj.ProductCode;
			searchObj.ProductName = productSearchObj.ProductName;

			//ViewBag.Product = _productServices.GetProducts(searchObj);
			ViewBag.Product = _productServices.GetProductsUsingEFCore();
			return View("ProductList");
		}

		[Authorize]
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
			_logger.LogInformation("Accessed Add New Product page.");

			var productCategories = _productServices.GetProductCategory();
			var productUOM = _productServices.GetUTM();
			ViewBag.ProductCategory = new SelectList(productCategories, "ProductCategoryId", "ProductCategoryName");
			ViewBag.ProductUOM = new SelectList(productUOM, "UOMIdPk", "UOMName");
			//SelectList is a class and helps you easily bind a collection to a dropdown list in your view.SelectList(collection,value,text)

			return View();
        }
		[Authorize]
		[HttpPost]
        public async Task<IActionResult> AddNewProduct(ProductViewModel model)
        {
			_logger.LogInformation("Attempting to add new product: {ProductName} with Code: {ProductCode}",
							model.ProductName, model.ProductCode);

			//return RedirectToAction("ProductList", "Product");
			ProductDTO productObj =new ProductDTO();
            productObj.ProductName = model.ProductName;
            productObj.ProductCode= model.ProductCode;
            
            productObj.PricePerUnit= model.PricePerUnit;
            productObj.ThresholdLimit=model.ThresholdLimit;
            
            productObj.CreatedBy = userName;
            productObj.CreatedOn = model.CreatedOn;
            
			bool result = await _productServices.SaveProduct(productObj);
			if (result)
			{
				_logger.LogInformation("Product {ProductName} added successfully.", model.ProductName);
			}
			else
			{
				_logger.LogWarning("Failed to add product: {ProductName}.", model.ProductName);
			}



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





		[Authorize]
		[HttpGet]
        public async Task<IActionResult> EditProduct(int ProductId)
        {
			_logger.LogInformation("Accessed Edit Product page for ProductId: {ProductId}", ProductId);

			ProductEditDTO productEditObj = new ProductEditDTO();
			productEditObj.ProductIdPk = ProductId;
			ViewBag.ProductDetails = await _productServices.GetProductBasedOnId(productEditObj);
			ProductEditViewModel productEditViewModelObj=new ProductEditViewModel();
			productEditViewModelObj.ProductIdPk= productEditObj.ProductIdPk;
			productEditViewModelObj.ProductName= productEditObj.ProductName;
			productEditViewModelObj.ProductCode = productEditObj.ProductCode;
			productEditViewModelObj.ThresholdLimit= productEditObj.ThresholdLimit;
			productEditViewModelObj.PricePerUnit= productEditObj.PricePerUnit;
			productEditViewModelObj.CategoryId = productEditObj.CategoryId;
			productEditViewModelObj.UnitofMeasurementId= productEditObj.UnitofMeasurementID;
			var productCategories = _productServices.GetProductCategory();
			var productUOM = _productServices.GetUTM();
			ViewBag.ProductCategory = new SelectList(productCategories, "ProductCategoryId", "ProductCategoryName", "CategoryId");
			ViewBag.ProductUOM = new SelectList(productUOM, "UOMIdPk", "UOMName", "UnitofMeasurementId");

			return View(productEditViewModelObj);
        }
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EditProduct(ProductEditViewModel productEditViewModelObj)
		{
			_logger.LogInformation("Attempting to update product: {ProductName}, ProductId: {ProductId}",
	productEditViewModelObj.ProductName, productEditViewModelObj.ProductIdPk);

			ProductEditDTO productEditObj = new ProductEditDTO();
			productEditObj.ProductIdPk = productEditViewModelObj.ProductIdPk;
			productEditObj.ProductName = productEditViewModelObj.ProductName;
			productEditObj.ProductCode = productEditViewModelObj.ProductCode;

			productEditObj.PricePerUnit = productEditViewModelObj.PricePerUnit;
			productEditObj.ThresholdLimit = productEditViewModelObj.ThresholdLimit;
			productEditObj.CategoryId= productEditViewModelObj.CategoryId;
			productEditObj.UnitofMeasurementID = productEditViewModelObj.UnitofMeasurementId;




			bool result = await _productServices.UpdateProducts(productEditObj);
			if (result)
			{
				_logger.LogInformation("Product {ProductName} updated successfully.", productEditViewModelObj.ProductName);
			}
			else
			{
				_logger.LogWarning("Failed to update product: {ProductName}.", productEditViewModelObj.ProductName);
			}


			ViewBag.Response = result;
			return View(productEditViewModelObj);


		}
		[Authorize]
		[HttpGet]
		
		public async Task<IActionResult> ViewProduct(int ProductId)
		{
			_logger.LogInformation("Accessed Edit Product page for ProductId: {ProductId}", ProductId);

			ProductEditDTO productEditObj = new ProductEditDTO();
			productEditObj.ProductIdPk = ProductId;
			ViewBag.ProductDetails = await _productServices.GetProductBasedOnId(productEditObj);
			ProductEditViewModel productEditViewModelObj = new ProductEditViewModel();
			productEditViewModelObj.ProductIdPk = productEditObj.ProductIdPk;
			productEditViewModelObj.ProductName = productEditObj.ProductName;
			productEditViewModelObj.ProductCode = productEditObj.ProductCode;
			productEditViewModelObj.ThresholdLimit = productEditObj.ThresholdLimit;
			productEditViewModelObj.PricePerUnit = productEditObj.PricePerUnit;
			productEditViewModelObj.CategoryId = productEditObj.CategoryId;
			productEditViewModelObj.UnitofMeasurementId = productEditObj.UnitofMeasurementID;
			var productCategories = _productServices.GetProductCategory();
			var productUOM = _productServices.GetUTM();
			ViewBag.ProductCategory = new SelectList(productCategories, "ProductCategoryId", "ProductCategoryName", "CategoryId");
			ViewBag.ProductUOM = new SelectList(productUOM, "UOMIdPk", "UOMName", "UnitofMeasurementId");

			return View(productEditViewModelObj);
		}
		[HttpPost]
        public IActionResult Save(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


        }
        
	}
}
