using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.OrderDtos
{
    public class GetAllOrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PayMethod { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
