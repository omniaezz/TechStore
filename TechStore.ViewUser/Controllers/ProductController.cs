using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.Models;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.ViewUser.Models;
using Microsoft.AspNetCore.Http;
using TechStore.ViewUser.ExtenstionMethods;

namespace TechStore.ViewUser.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IReviewService _ReviewService;

       
        public ProductController(IProductService productService ,IReviewService reviewService)
        {
            _productService = productService;
            _ReviewService= reviewService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Mobile(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.CategoryId = 1;
            var brands = await _productService.GetBrands(1);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months","30 Days" };

            var products = await _productService.FilterProductsByCategory(1, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Mobile";
            return View("ProductsByCategory", products);
        }
        public async Task<IActionResult> Laptop(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.CategoryId = 2;

            var brands = await _productService.GetBrands(2);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };

            var products = await _productService.FilterProductsByCategory(2, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Laptop";
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> Screen(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.CategoryId = 3;

            var brands = await _productService.GetBrands(3);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };

            var products = await _productService.FilterProductsByCategory(3, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Screen";
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> SmartWatch(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.CategoryId = 4;
            var brands = await _productService.GetBrands(4);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };

            var products = await _productService.FilterProductsByCategory(4, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "SmartWatch";
            return View("ProductsByCategory",products);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name, int pageSize = 9, int pageNumber = 1)
        {
            try
            {
                var result = await _productService.SearchProduct(name, pageSize, pageNumber);
                ViewBag.PageNumber = pageNumber;
                ViewBag.Name = name;
                return View(result);
            }
            catch (ArgumentException ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }
    
        public async Task<IActionResult> Filter(FillterProductsDtos criteria, int categoryId, int itemsPerPage = 9, int pageNumber = 1)
        {
            try
            {
                var brands = await _productService.GetBrands(categoryId);
                ViewBag.Brands = brands;
                ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };

                if (ViewBag.Brands == null)
                {
                    ViewBag.Brands = new List<string>(); 
                }
                ViewBag.CategoryId = categoryId;
                ViewBag.PageNumber = pageNumber;
                ViewBag.Criteria = criteria;

                var result = await _productService.FilterProducts(criteria,categoryId,itemsPerPage,pageNumber);
                return View("ProductsByCategory", result);
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel { Message = "An error occurred while processing your request." });
            }
        }
        
        public async Task<IActionResult> Details(int id )
        {
            
           
            try
            {
                string UserId = User.Identity.Name;
             
                var result = await _productService.GetOne(id);
                     
                return View("Details",result);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message=ex.Message });
            }
        }


        public async Task<IActionResult> SortByAscending(int categoryId, int pageNumber = 1)
        {
            var result = await _productService.SortProductsByAscending(categoryId, 9, pageNumber);
            ViewBag.CategoryId = categoryId;
            ViewBag.PageNumber = pageNumber;
            var brands = await _productService.GetBrands(categoryId);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };


            return View("ProductsByCategory", result);
        }

        public async Task<IActionResult> SortByDescending(int categoryId, int pageNumber = 1)
        {

            var result = await _productService.SortProductsByDesending(categoryId, 9, pageNumber);
            ViewBag.CategoryId = categoryId;
            ViewBag.PageNumber = pageNumber;
            var brands = await _productService.GetBrands(categoryId);
            ViewBag.Brands = brands;
            ViewBag.WarrantyOptions = new List<string> { "1 Year", "2 Years", "3 Months", "6 Months", "30 Days" };

            return View("ProductsByCategory", result);
        }


    }

}

