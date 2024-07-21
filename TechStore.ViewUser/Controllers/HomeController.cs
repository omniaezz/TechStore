using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TechStore.Application.Services;
using TechStore.Models;
using TechStore.ViewUser.Models;

namespace TechStore.ViewUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
             private IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult>  Index()
        {
            var productsResultTask = _productService.FilterNewlyAddedProducts(30);
            var productsResult = await productsResultTask;

            ViewBag.Products1 = productsResult.Entities.Take(7);
            ViewBag.Products2 = productsResult.Entities.Skip(7).Take(7);

            ViewBag.Products3 = productsResult.Entities.Skip(14).Take(7);
            

            //ViewBag.Products5 = productsResult.Entities.Skip(20).Take(5);
            //ViewBag.Products6 = productsResult.Entities.Skip(25).Take(5);


            var TopOfers = _productService.FilterDiscountedProducts();
            var TopOfersResult = await TopOfers;
            ViewBag.TopOfers1 = TopOfersResult.Entities.Take(10);


            // تحديد اتجاه الصفحة بناءً على اللغة المحددة
            if (CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
            {
                ViewData["PageDirection"] = "rtl";
            }
            else
            {
                ViewData["PageDirection"] = "ltr";
            }

            return View();
            }



           
        public IActionResult Cart()
        {
            return View("Details","Product");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) });
            return LocalRedirect(returnUrl);
        }
    }
}
