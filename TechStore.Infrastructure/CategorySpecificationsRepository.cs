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
    public class CategorySpecificationsRepository:Repository<CategorySpecifications,int>,ICategorySpecificationsRepository
    {
        public CategorySpecificationsRepository(TechStoreContext context):base(context) { }

        public Task<CategorySpecifications> GetSpecByCategoryAndSpecId(int CategoryId,int SpecificationId)
        {
            return Task.FromResult(_entities.Where(s => s.CategoryId == CategoryId && s.SpecificationId == SpecificationId).FirstOrDefault());
        }

    }
}
