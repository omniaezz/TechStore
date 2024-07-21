using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Shipped = 1,
        Delivered = 2,
    }
    public class Order : BaseEntity
    {
        public string? UserId { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public string? ShippingAddress { get; set; }
        public string? PayMethod { get; set; }
        public string? Phone { get; set; }
        public decimal? TotalPrice { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? DeliveryDate { get; set; } = DateTime.Now.AddDays(3);

        // Define relationships
        public TechUser? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Payment>? Payments { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
