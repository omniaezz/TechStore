using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.AccountDtos
{
    public class LoginDto
    {

        [Required(ErrorMessage = "User Name is Required")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password  is Required")]
        public string Password { get; set; }
    }
}
