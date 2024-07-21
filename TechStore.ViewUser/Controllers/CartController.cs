using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.ViewUser.ExtenstionMethods;

namespace TechStore.ViewUser.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;


        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var sessionCartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var productsResultTask = _productService.FilterNewlyAddedProducts(10);
            var productsResult = await productsResultTask;

            ViewBag.Products = productsResult.Entities.Take(10);
            ViewBag.Products2 = productsResult.Entities.Skip(5).Take(5);
            return View("CartTest", sessionCartItems);
        }



        public IActionResult AddToCart(CartItemDto cartItemDto)        
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == cartItemDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                cart.Add(cartItemDto);

            }
            int cartItemCount = GetCartItemCounts();
            ViewBag.CartItemCount = cartItemCount;
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Cart");

        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                if (itemToRemove.Quantity > 1)
                {
                    itemToRemove.Quantity--;
                }
                else
                {
                    cart.Remove(itemToRemove);
                }
                HttpContext.Session.Set("Cart", cart);
            }
            int cartItemCount = cart.Sum(item => item.Quantity);
            ViewBag.CartItemCount = cartItemCount;
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;

                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction("Index","Cart");
        }

        public IActionResult Cart()
        {
            var sessionCartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            return View("Cart", sessionCartItems);
        }

        private int GetCartItemCounts()
        {
            // Retrieve cart count from session or database
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            int cartItemCount = cart.Sum(item => item.Quantity);
            return cartItemCount;
        }
        public IActionResult GetCartItemCount()
        {
            int cartItemCount = GetCartItemCounts();
            return Json(cartItemCount);
        }
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
