using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
         
        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        
        public async Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.OrderDate = DateTime.Now;

                var orderItems = _mapper.Map<List<OrderItem>>(orderDto.OrderItems);

                foreach (var orderItem in orderItems)
                {
                    var correspondingDto = orderDto.OrderItems.FirstOrDefault(dto => dto.Id == orderItem.Id);
                    orderItem.UnitPrice = correspondingDto.UnitePrice;

                    var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                    if(product.Quantity < orderItem.Quantity)
                        throw new Exception($"Product'Quantity Isn't Valid");
                    product.Quantity -= orderItem.Quantity ?? 0;
                    await _productRepository.UpdateAsync(product);
                    await _productRepository.SaveChangesAsync();
                }

                order.TotalPrice = orderDto.TotalPrice;
                order.OrderItems = orderItems;

                var createdOrder = await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveChangesAsync();
                await _orderItemRepository.SaveChangesAsync();

                var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);

                result.IsSuccess = true;
                result.Message = "Order created successfully.";
                result.Entity = createdOrderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to create order: {ex.Message}";
            }

            return result;
        }

        
        public async Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity)
        {
            var result = new ResultView<OrderItemDto>();

            try
            {
                var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
                if (orderItem == null)
                    throw new Exception("Order item not found.");

                var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                if (product == null)
                    throw new Exception("Product not found.");

                int quantityDifference;
                if (newQuantity > orderItem.Quantity)
                {
                    quantityDifference = newQuantity - (orderItem.Quantity ?? 0);
                    product.Quantity -= quantityDifference;
                }
                else
                {
                    quantityDifference = (orderItem.Quantity ?? 0) - newQuantity;
                    product.Quantity += quantityDifference;
                }

                orderItem.Quantity = newQuantity;

                await _orderItemRepository.UpdateAsync(orderItem);
                await _orderItemRepository.SaveChangesAsync();
                await _productRepository.UpdateAsync(product);
                await _productRepository.SaveChangesAsync();


                var updatedOrderItemDto = _mapper.Map<OrderItemDto>(orderItem);

                result.IsSuccess = true;
                result.Message = "Order item quantity updated successfully.";
                result.Entity = updatedOrderItemDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to update order item quantity: {ex.Message}";
            }

            return result;
        }

        
        public async Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId)
        {
            var result = new ResultView<GetAllOrderDto>();

            try
            {
                var order = (await _orderRepository.GetAllAsync())
                            .Include(order => order.User)
                            .FirstOrDefault(order => order.Id == orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found.");

                var orderDto = _mapper.Map<GetAllOrderDto>(order);

                result.IsSuccess = true;
                result.Message = "Order retrieved successfully.";
                result.Entity = orderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to retrieve order: {ex.Message}";
            }

            return result;
        }

        
        public async Task<ResultDataList<GetAllOrderItemDto>> GetOrderItems(int orderId)
        {
            var ExistingOrder = await _orderRepository.GetByIdAsync(orderId);

            if (ExistingOrder != null)
            {
                var OrderItems = await _orderItemRepository.GetOrders(orderId);

                var list = new List<GetAllOrderItemDto>();

                foreach (var OrderItem in OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(OrderItem.ProductId);
                    var images = await _productRepository.GetImagesByProductId(OrderItem.ProductId);
                    product.Images = images.ToList();
                    var obj = new GetAllOrderItemDto
                    {
                        Id = OrderItem.Id,
                        OrderId = OrderItem.OrderId,
                        ProductId = OrderItem.ProductId,
                        Description = OrderItem.Product.Description,
                        Ar_Description = OrderItem.Product.Ar_Description,
                        Price = OrderItem.Product.Price,
                        Quantity = OrderItem.Quantity,
                        Image = product.Images.Select(i => i.Name).FirstOrDefault()
                    };
                    list.Add(obj);
                }

                var listDto = _mapper.Map<List<GetAllOrderItemDto>>(list);


                return new ResultDataList<GetAllOrderItemDto>()
                {
                    Entities = listDto,
                    Count = listDto.Count()
                };

            }
            else
            {
                return new ResultDataList<GetAllOrderItemDto>()
                {
                    Entities = null,
                    Count = 0
                };
            }
        }

        public async Task<ResultView<GetOrderWithItemsDto>> GetOrderDetails(int orderId)
        { 
            var ExistingOrder = await _orderRepository.GetByIdAsync(orderId);

            if(ExistingOrder != null)
            {
                var OrderItems = await _orderItemRepository.GetOrders(orderId);

                var list = new List<GetOrderDetailsDto>();


                foreach (var OrderItem in OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(OrderItem.ProductId);
                    var images = await _productRepository.GetImagesByProductId(OrderItem.ProductId);
                    product.Images = images.ToList();
                    var obj = new GetOrderDetailsDto
                    {
                        Id = OrderItem.Id,
                        OrderId = OrderItem.OrderId,
                        ProductId = OrderItem.ProductId,
                        Description = OrderItem.Product.Description,
                        Price = OrderItem.Product.Price,
                        Quantity = OrderItem.Quantity,
                        Ar_Description = OrderItem.Product.Ar_Description,
                        Image = product.Images.Select(i => i.Name).FirstOrDefault()
                    };
                    list.Add(obj);
                }

                var orderDto = _mapper.Map<OrderWithoutItemsDto>(ExistingOrder);
                var listDto = _mapper.Map<List<GetOrderDetailsDto>>(list);

                var orderWithItemsDto = new GetOrderWithItemsDto { order = orderDto, Details = listDto };

                return new ResultView<GetOrderWithItemsDto>()
                {
                    Entity = orderWithItemsDto,
                    IsSuccess = true,
                    Message = "Order Retrived Successfully"
                };

            }
            else
            {
                return new ResultView<GetOrderWithItemsDto>()
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Faild To Retrive Order"
                };
            }
        }


        public async Task<ResultDataList<GetAllOrderDto>> GetAllPaginationOrders(int ItemsPerPage, int PageNumber)
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetAllAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }
        
        
        public async Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found.");

                order.IsDeleted = true;
                await _orderRepository.UpdateAsync(order);

                var OrderItems = await _orderItemRepository.GetOrders(orderId);//how binding in order ????
                foreach (var item in OrderItems)
                {
                    item.IsDeleted = true;
                    await _orderItemRepository.UpdateAsync(item);
                }
                
                await _orderRepository.SaveChangesAsync();
                await _orderItemRepository.SaveChangesAsync();
                var orderDto = _mapper.Map<OrderDto>(order);

                result.IsSuccess = true;
                result.Message = "Order deleted successfully.";
                result.Entity = orderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to delete order: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultView<OrderItemDto>> SoftDeleteOrderItemAsync(int orderItemId)
        {
            var result = new ResultView<OrderItemDto>();
            var ExistingOrderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if(ExistingOrderItem != null)
            {
                ExistingOrderItem.IsDeleted = true;
                await _orderItemRepository.UpdateAsync(ExistingOrderItem);
                await _orderItemRepository.SaveChangesAsync();
                var orderItemDto = _mapper.Map<OrderItemDto>(ExistingOrderItem);
                result.Entity = orderItemDto;
                result.IsSuccess = true;
                result.Message = "OrderItem Deleted Successfully";

            }
            else
            {
                result.Entity = null;
                result.IsSuccess = false;
                result.Message = "Faild To Delete OrderItem";
            }
            return result;
        }
        

        public async Task<ResultView<OrderDto>> HardDeleteOrderAsync(int orderId)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var deletedOrder = await _orderRepository.GetByIdAsync(orderId);
                deletedOrder.IsDeleted = true;
                if (deletedOrder == null)
                    throw new Exception($"Order with ID {deletedOrder.Id} not found.");

                var deletedOrderItems = await _orderItemRepository.GetOrders(orderId);
                foreach (var item in deletedOrderItems)
                {
                    item.IsDeleted = true;//to make returned with this value
                    await _orderItemRepository.DeleteAsync(item);
                }

                await _orderItemRepository.SaveChangesAsync();  //delete orderitems then delete order
                await _orderRepository.DeleteAsync(deletedOrder);
                await _orderRepository.SaveChangesAsync();

                deletedOrder.OrderItems = deletedOrderItems;
                var deletedOrderDto = _mapper.Map<OrderDto>(deletedOrder);

                result.IsSuccess = true;
                result.Message = "Order  deleted successfully.";
                result.Entity = deletedOrderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to  delete order: {ex.Message}";
            }

            return result;
        }

        
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateAscendingAsync()
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersSortedByDateAscendingAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }

        
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateDescendingAsync()
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersSortedByDateDescendingAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }

        
        public async Task<ResultDataList<GetAllOrderDto>> SearchOrdersAsync(int searchTerm)
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.SearchOrdersAsync(searchTerm);
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }

        
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
               
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
                
            }

            return result;
        }

       
        public async Task<ResultView<OrderDto>> updateStatus(int OrderId , OrderStatus NewOrderStatus)
        {
            var ExistingOrder = await _orderRepository.GetByIdAsync(OrderId);
            var result = new ResultView<OrderDto>();
            if (ExistingOrder != null)
            {
                ExistingOrder.OrderStatus = NewOrderStatus;
                await _orderRepository.UpdateAsync(ExistingOrder);
                await _orderRepository.SaveChangesAsync();
                var orderItems =await _orderItemRepository.GetOrders(OrderId);
                ExistingOrder.OrderItems = orderItems;
                var UpdatedOrderDto = _mapper.Map<OrderDto>(ExistingOrder);

                result.Entity = UpdatedOrderDto;
                result.IsSuccess = true;
                result.Message = "OrderStatus Updated Successfully";
            }
            else
            {
                result.Entity = null;
                result.IsSuccess = false;
                result.Message = "Failed to update order status";
            }
            return result;
        }
    
    }

}
