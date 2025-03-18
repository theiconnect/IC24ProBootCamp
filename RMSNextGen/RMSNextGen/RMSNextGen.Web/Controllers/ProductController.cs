﻿using Microsoft.AspNetCore.Mvc;
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

            bool result=await _productServices.SaveProduct(productObj);

            //ViewBag.Message = result ? "Product Added Successfully" : "Unable to Add Product";
            ViewBag.Response = result;


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
