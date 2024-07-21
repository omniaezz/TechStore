using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.UserDTO
{
    public class GetAllUserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
    }
}
