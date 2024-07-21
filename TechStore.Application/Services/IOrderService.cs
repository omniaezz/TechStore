using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IOrderService
    {
        Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto);
        Task<ResultDataList<GetAllOrderDto>> GetAllPaginationOrders(int ItemsPerPage, int PageNumber);
        Task<ResultDataList<GetAllOrderItemDto>> GetOrderItems(int orderId);
        Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId);
        Task<ResultView<OrderDto>> HardDeleteOrderAsync(int orderId);
        Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId);
        Task<ResultView<OrderItemDto>> SoftDeleteOrderItemAsync(int orderItemId);
        Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity);
        Task<ResultView<OrderDto>> updateStatus(int OrderId, OrderStatus NewOrderStatus);
        Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateAscendingAsync();
        Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateDescendingAsync();
        Task<ResultDataList<GetAllOrderDto>> GetOrdersByUserIdAsync(string userId);
        Task<ResultDataList<GetAllOrderDto>> SearchOrdersAsync(int searchTerm);
    }
}