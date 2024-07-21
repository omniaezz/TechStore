using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface ICategorySpecificationsRepository:IRepository<CategorySpecifications,int>
    {
        Task<CategorySpecifications> GetSpecByCategoryAndSpecId(int CategoryId, int SpecificationId);
    }
}
