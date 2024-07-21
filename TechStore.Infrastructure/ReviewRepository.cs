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
    public class ReviewRepository:Repository<Review,int>,IReviewRepository
    {
      
        public ReviewRepository(TechStoreContext context):base(context)
        {
        
        }
       
    }
}
