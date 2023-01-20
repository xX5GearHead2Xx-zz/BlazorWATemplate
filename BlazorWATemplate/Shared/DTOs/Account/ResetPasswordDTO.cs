using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWATemplate.Shared.DTOs.Account
{
    public class ResetPasswordDTO
    {
        [Required, PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
        [Required, PasswordPropertyText, Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PasswordRecoveryID { get; set; } = string.Empty;
    }
}
