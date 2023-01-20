using System;
using System.Collections.Generic;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class User
    {
        public User()
        {
            LinkUserRoles = new HashSet<LinkUserRole>();
            PasswordRecoveries = new HashSet<PasswordRecovery>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public byte[] Password { get; set; } = null!;
        public byte[]? Salt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastActive { get; set; }

        public virtual ICollection<LinkUserRole> LinkUserRoles { get; set; }
        public virtual ICollection<PasswordRecovery> PasswordRecoveries { get; set; }
    }
}
