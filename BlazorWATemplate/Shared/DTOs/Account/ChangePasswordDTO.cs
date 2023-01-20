using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWATemplate.Shared.DTOs.Account
{
    public class ChangePasswordDTO
    {
        [Required, PasswordPropertyText]
        public string OldPassword { get; set; } = string.Empty;
        [Required, PasswordPropertyText]
        public string NewPassword { get; set; } = string.Empty;
        [Required, PasswordPropertyText]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
