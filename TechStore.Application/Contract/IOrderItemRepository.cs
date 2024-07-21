using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IOrderItemRepository:IRepository<OrderItem,int>
    {
        public Task<List<OrderItem>> GetOrders(int orderId);

    }
}
