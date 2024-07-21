using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }

        public ICollection<CategorySpecifications> CategorySpecifications { get; set; }
        
        public ICollection<ProductCategorySpecifications> ProductCategorySpecifications { get; set; }
        public Category ()
        {
            Products = new List<Product> ();
            CategorySpecifications = new List<CategorySpecifications>();
            ProductCategorySpecifications = new List<ProductCategorySpecifications>();

        }
    }
}
