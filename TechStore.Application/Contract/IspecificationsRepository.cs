using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IspecificationsRepository:IRepository<Specification,int>
    {
        Task<Specification> SearchByName(string Name);

        Task<IQueryable<Specification>> GetSpecificationsByCategory(int categoryId);

        Task<string> GetSpecificationNameById(int id);
    }
}
