using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface ICategoryRepository:IRepository<Category,int>
    {
        Task<Category> SearchByName(string name);

        Task DetachEntityAsync(Category entity);
        
    }
}
