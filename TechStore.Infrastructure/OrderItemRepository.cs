using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class OrderItemRepository : Repository<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(TechStoreContext context) : base(context)
        {

        }
        public  Task<List<OrderItem>> GetOrders(int orderId)
        {
            return Task.FromResult( _entities.Where(order=>order.OrderId == orderId).ToList());
        }
    }
}
