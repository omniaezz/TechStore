using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class ProductCategorySpecificationsRepository:Repository<ProductCategorySpecifications,int>,IproductCategorySpecifications
    {
        public ProductCategorySpecificationsRepository(TechStoreContext context) : base(context) { }

        public async Task<IQueryable<ProductCategorySpecifications>> GetProductCategorySpecifications(int productId)
        {
            return _context.ProductSpecifications.Where(ProductSpec => ProductSpec.ProductId == productId);
        }


    }
}
