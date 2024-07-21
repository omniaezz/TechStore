using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos
{
    public class GetAllProductsForUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public ICollection<string> Images { get; set; }
        public double AverageRating { get; set; }
        public DateTime DateAdded { get; set; }
        public string WarrantyInformation { get; set; }
        public string color { get; set; }
        public string CategoryName { get; set; }
    }
}
