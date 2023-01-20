using System;
using System.Collections.Generic;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class PasswordRecovery
    {
        public int PasswordRecoveryId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Expiry { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
