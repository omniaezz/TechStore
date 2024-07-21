using Microsoft.AspNetCore.Mvc;
using TechStore.Dtos.ProductDtos;
using Microsoft.AspNetCore.Http;
using TechStore.ViewUser.ExtenstionMethods;
using Stripe.Checkout;
using Stripe;
using TechStore.Application.Services;
using Stripe.Issuing;
using TechStore.Dtos.OrderDtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TechStore.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace TechStore.ViewUser.Controllers
{
    //[Authorize]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<TechUser> _userManager;

        public PaymentController(IOrderService orderService, UserManager<TechUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View("PayDetails");
        }

        public async Task<IActionResult> OrderConfirmationAsync()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            var CartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            
            if (session.PaymentStatus == "paid")
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var UserDetails = HttpContext.Session.Get<List<string>>("UserDetails") ?? new List<string>();
                string address = UserDetails[0];
                string phoneNumber = UserDetails[1];

                decimal TotalPrice = 0;

                var orderItems = new List<OrderItemDto>();
                foreach (var item in CartItems)
                {
                    var orderItem = new OrderItemDto { ProductId = item.ProductId, Quantity = item.Quantity ,UnitePrice = item.DiscountedPrice };
                    TotalPrice += (item.DiscountedPrice * item.Quantity);
                    orderItems.Add(orderItem);
                }

                var orderdto = new OrderDto
                {
                    UserId = userId,
                    ShippingAddress = address,
                    OrderStatus = "Pending",
                    PayMethod = "Stripe",
                    OrderItems = orderItems,
                    TotalPrice = TotalPrice,
                    Phone = phoneNumber
                };
                var order = await _orderService.CreateOrderAsync(orderdto);
                if(order.IsSuccess = false)
                {
                    return View("Success");
                }
                var transaction = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("Cancel");
        }

        public async Task<IActionResult> CheckOut(string Address, string PhoneNumber,string paymentMethod)
        {
            if(paymentMethod == "Cash")
            {
                return RedirectToAction("PayWithCash", new { Address = Address, PhoneNumber = PhoneNumber });
            }

            var CartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var Details = new List<string>();
            Details.Add(Address);
            Details.Add(PhoneNumber);
            HttpContext.Session.Set("UserDetails",Details);


            var domain = "http://localhost:5112/";

            var Options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Payment/OrderConfirmation",
                CancelUrl = domain + $"Payment/OrderConfirmation",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in CartItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long) item.DiscountedPrice * 100,
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Description.ToString(),
                        }
                    },
                    Quantity = item.Quantity,
                };
                Options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = await service.CreateAsync(Options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> PayWithCash(string Address,string PhoneNumber)
        {
            var CartItems = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderItems = new List<OrderItemDto>();
            decimal TotalPrice = 0;
            foreach (var item in CartItems)
            {
                var orderItem = new OrderItemDto { ProductId = item.ProductId, Quantity = item.Quantity, UnitePrice = item.DiscountedPrice };
                TotalPrice += (item.DiscountedPrice * item.Quantity);
                orderItems.Add(orderItem);
            }

            var orderdto = new OrderDto
            {
                UserId = userId,
                ShippingAddress = Address,
                OrderStatus = "Pending",
                PayMethod = "Cash",
                OrderItems = orderItems,
                TotalPrice = TotalPrice,
                Phone = PhoneNumber
            };
            var orders = await _orderService.CreateOrderAsync(orderdto);
            return View("SuccessCash");
        }
    }
}
