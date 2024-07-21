using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.UserDTO
{
    public class CreateOrUpdateUserDTO
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? image { get; set; }
        public string? AccountStatus { get; set; }
       

    }
    
}
