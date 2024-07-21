using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.OrderDtos
{
    public class GetAllOrderItemDto
    {
        public int Id { get; set; }//orderitem
        public int? ProductId { get; set; }//product
        public int? OrderId { get; set; }//order
        public string? Image { get; set; }//product
        public string? Description { get; set; }//product
        public string? Ar_Description { get; set; }//product
        public decimal? Price { get; set; }//product
        public int? Quantity { get; set; }//orderitem
    }
}
