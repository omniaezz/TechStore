using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IOrderRepository:IRepository<Order,int>
    {
        Task<List<Order>> GetOrdersSortedByDateAscendingAsync();
        Task<List<Order>> GetOrdersSortedByDateDescendingAsync();
        Task<List<Order>> SearchOrdersAsync(int searchTerm);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);

    }
}
