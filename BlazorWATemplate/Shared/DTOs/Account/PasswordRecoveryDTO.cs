using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWATemplate.Shared.DTOs.Account
{
    public class PasswordRecoveryDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please complete the recaptcha")]
        public string RecaptchaResponse { get; set; } = string.Empty;
    }
}