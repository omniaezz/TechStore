using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Specifications { get; set; } 
        public string WarrantyInformation { get; set; }
        public List<string> Images { get; set; }
        public double AverageRating { get; set; }
        //public ICollection<Review> Reviews { get; set; }
        public DateTime DateAdded { get; set; }
        public int categoryId { get; set; }
        //public Category category { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
