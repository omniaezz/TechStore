using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IproductCategorySpecifications:IRepository<ProductCategorySpecifications,int>
    {
        Task<IQueryable<ProductCategorySpecifications>> GetProductCategorySpecifications(int productId);
    }
}
