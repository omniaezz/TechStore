using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Dtos.ProductDtos
{
    public class CreateOrUpdateProductDtos
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountedPrice => Price * DiscountValue / 100;
        public string? Warranty { get; set; }
        public int? Quantity { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? Ar_Description { get; set; }
        public string? Ar_ModelName { get; set; }
    }
}
