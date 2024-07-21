using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] OrderDto orderDto)
        {
            var product = await _orderService.CreateOrderAsync(orderDto);
            return Ok(product);
        }

        [HttpPut("update")]
        public async Task<IActionResult> update( int orderId, int orderItemId, int newQuantity)
        {
            var product = await _orderService.UpdateOrderItemQuantityAsync(orderId,orderItemId,newQuantity);
            return Ok(product);
        }

        [HttpGet("GetAllOrdersAsync")]
        public async Task<IActionResult> GetAllOrdersAsync(int pageItem = 10 , int PageNumber = 1)
        {
            var result = await _orderService.GetAllPaginationOrders(pageItem, PageNumber);
            return Ok(result);
        }

        [HttpGet("GetOrderItems")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var result = await _orderService.GetOrderItems(orderId);
            return Ok(result);
        }
        
        [HttpDelete("SoftDeleteOrder")]
        public async Task<IActionResult> SoftDeleteOrder(int id)
        {
            var result  = await _orderService.SoftDeleteOrderAsync(id);
            return Ok(result);
        }

        [HttpDelete("SoftDeleteOrderItem")]
        public async Task<IActionResult> SoftDeleteOrderItem(int id)
        {
            var result = await _orderService.SoftDeleteOrderItemAsync(id);
            return Ok(result);
        }

        [HttpDelete("HardDeleteOrder")]
        public async Task<IActionResult> HardDeleteOrder(int id)
        {
            var result = await _orderService.HardDeleteOrderAsync(id);
            return Ok(result);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> updateStatus(int OrderId, Models.OrderStatus NewOrderStatus)
        {
            var state = await _orderService.updateStatus(OrderId, NewOrderStatus);
            return Ok(state);
        }

        [HttpGet("GetSortedAs")]
        public async Task<IActionResult> GetSortedAs()
        {
            var result = await _orderService.GetOrdersSortedByDateAscendingAsync();
            return Ok(result);
        }

        [HttpGet("GetSortedes")]
        public async Task<IActionResult> GetSortedes()
        {
            var result = await _orderService.GetOrdersSortedByDateDescendingAsync();
            return Ok(result);
        }

        [HttpGet("GetorderByUserId")]
        public async Task<IActionResult> GetorderByUserId(string userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("searchOrder")]
        public async Task<IActionResult> searchOrder(int searchTerm)
        {
            var result = await _orderService.SearchOrdersAsync(searchTerm);
            return Ok(result);
        }
    }
}
