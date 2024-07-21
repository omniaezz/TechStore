using Microsoft.EntityFrameworkCore;
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
    public class SpecificationsRepository : Repository<Specification,int>,IspecificationsRepository
    {
        public SpecificationsRepository(TechStoreContext context):base(context)
        {

        }

        public async Task<Specification> SearchByName(string name)
        {
            return await _entities.Where(c => c.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Specification>> GetSpecificationsByCategory(int categoryId)
        {
            var specifications = from categorySpec in _context.CategorySpecifications
                                 join spec in _context.Specifications on categorySpec.SpecificationId equals spec.Id
                                 where categorySpec.CategoryId == categoryId
                                 select spec;

            return specifications;
        }

        public Task<string> GetSpecificationNameById(int id)
        {
            return Task.FromResult( _entities.Where(spec=>spec.Id == id).Select(se=>se.Name).FirstOrDefault());
        }
    }
}
