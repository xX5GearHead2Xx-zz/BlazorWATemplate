using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWATemplate.Shared.DTOs.Shared
{
    public class LoginDTO
    {
        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please complete the recaptcha")]
        public string RecaptchaResponse { get; set; } = string.Empty;
    }
}
