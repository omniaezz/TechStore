using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Dtos.OrderDtos;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(TechStoreContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetOrdersSortedByDateAscendingAsync()
        {
            return await _context.Orders.OrderBy(order => order.OrderDate).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersSortedByDateDescendingAsync()
        {
            return await _context.Orders.OrderByDescending(order => order.OrderDate).ToListAsync();
        }

        public Task<List<Order>> SearchOrdersAsync(int searchTerm)
        {

            var filteredOrders = _context.Orders
                .Where(order =>
                      (int) order.OrderStatus == searchTerm
                ).ToList();

            return Task.FromResult( filteredOrders);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(order => order.UserId == userId && order.IsDeleted == false)
                .ToListAsync();
        }
    }
}
