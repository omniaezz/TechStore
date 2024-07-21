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
    public class UserRepository : Repository<TechUser,string>, IUserRepository
    {
       
        public UserRepository(TechStoreContext context) : base(context)
        {
           
        }
        public async Task<bool> IsUserNameExists(string userName)
        {
            // You need to replace YourDbContext with your actual DbContext class
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
        public async Task<List<TechUser>> SearchUserByName(string name)
        {
            var data= await _context.Users.Where(u=>u.FirstName ==name|| u.LastName==name).Select(u=>u).ToListAsync();
            return data;
        }

      
    }
}
