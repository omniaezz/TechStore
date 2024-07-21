using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.UserDTO
{
    public class UpdateUserDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [DisplayName("User Name")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Phone Number  is Required")]
        [DisplayName("Phone Number")]

        public string PhoneNumber { get; set; }

        [EmailAddress, Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = " Address is Required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Image is Required")]

        public IFormFile UrlImage { get; set; }
        public string?Image { get; set; }


    }
    
}
