using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.AccountDtos
{
    public class RegisterDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [MaxLength(50, ErrorMessage = "User Name cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Image is Required")]
        public IFormFile Image { get; set; }
    }
}
