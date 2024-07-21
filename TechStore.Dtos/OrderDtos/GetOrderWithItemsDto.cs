using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.OrderDtos
{
    public class GetOrderWithItemsDto
    {
        public OrderWithoutItemsDto order { get; set; }
        public List<GetOrderDetailsDto> Details { get; set; }
    }
}
