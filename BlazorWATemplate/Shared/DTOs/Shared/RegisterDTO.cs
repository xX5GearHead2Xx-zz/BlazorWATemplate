using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWATemplate.Shared.DTOs.Shared
{
    public class RegisterDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 8, ErrorMessage = "A minimum length of 8 is required")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please complete the recaptcha")]
        public string RecaptchaResponse { get; set; } = string.Empty;
    }
}
