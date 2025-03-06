using Microsoft.AspNetCore.Mvc;
using RMSNextGen.Web.Models;
using RMSNextGen.Models;
using RMSNextGen.Services;

namespace RMSNextGen.Web.Controllers
{
    public class ProductController : Controller
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
		
		
		[HttpGet]
        public IActionResult AddNewProduct()
        {
			//ViewBag.ProductCode = _productServices.GetProductCode();
			//string productCode = Convert.ToString(ViewBag.ProductCode);

			//if (String.IsNullOrEmpty(productCode))
			//{
			//	ViewBag.ProductCode = "P-001";

			//}
			//else
			//{
			//	string[] productCodeArray = productCode.Split('-');

			//	productCodeArray[1] = (productCodeArray[1]) + 1;

			//	productCode = productCodeArray[0] + productCodeArray[1];

			//}
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



            //ViewBag.Message = result ? "Product Added Successfully" : "Unable to Add Product";

            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await _productServices.SaveProduct(productObj);

                    ViewBag.Response = result;
                    if (result)
                    {
                        return RedirectToAction("AddNewProduct", "Product");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Product Details.");
                        return View(model);
                    }



                }
                


            }
            catch (Exception ex) 
            {
                throw;
            }
			ViewBag.Message = "Product Details Not Saved Successfully";

			return View(model);



		}
        [HttpGet]
        public IActionResult EditProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditProduct(IFormCollection form)
        {
            return RedirectToAction("ProductList", "Product");


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
