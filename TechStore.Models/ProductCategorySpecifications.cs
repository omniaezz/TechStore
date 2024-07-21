using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class ProductCategorySpecifications:BaseEntity
    {
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("Specification")]
        public int? SpecificationId { get; set; }
        public Specification? Specification { get; set; }

        public string Value { get; set; }
    }
}
