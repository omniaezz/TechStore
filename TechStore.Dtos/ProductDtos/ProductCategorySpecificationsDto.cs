using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Dtos.ProductDtos
{
    public class ProductCategorySpecificationsDto
    {
        public int Id {  get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecificationId { get; set; }
        public string Value { get; set; }
    }
}
