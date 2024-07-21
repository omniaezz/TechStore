using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class FillterProductsDtos
    {
        public string? Brand {  get; set; }
        public string? Warranty {  get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? DiscountValue { get; set;}
    }
}
