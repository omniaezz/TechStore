using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.Models;
using TechStore.ViewUser.ExtenstionMethods;


namespace TechStore.ViewUser.Controllers
{
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<TechUser> _userManager;

        public OrderController(IOrderService orderService, UserManager<TechUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            // Get user's address and phone number
            var address = user.Address;
            var phone = user.PhoneNumber;
            ViewBag.Address = address;
            ViewBag.Phone = phone;
            ViewBag.UserId = userId;
            var cartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
         
            return View("checkout",cartItems);
        }

        public async Task<IActionResult> GetAllOrders(string UserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = userId;
            var Orders = await _orderService.GetOrdersByUserIdAsync(UserId);
            return View("Orders", Orders);
        }

        public async Task<IActionResult> GetAllOrderItems(int OrderId)
        {
            var OrderItems = await _orderService.GetOrderItems(OrderId);
            return View("OrderItems", OrderItems);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int OrderId)
        {
            await _orderService.SoftDeleteOrderAsync(OrderId);
            return RedirectToAction("GetAllOrders");
        }


    }
}
