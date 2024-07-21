using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class ProductCategorySpecificationsListDto
    {
        public CreateOrUpdateProductDtos CreateOrUpdateProductDtos { get; set; }
        public List<ProductCategorySpecificationsDto> ProductCategorySpecifications { get; set; }
    }
}
