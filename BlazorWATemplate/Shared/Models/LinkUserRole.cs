using System;
using System.Collections.Generic;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class LinkUserRole
    {
        public int LinkUserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AccessTypeId { get; set; }

        public virtual ReferenceUserAccessType AccessType { get; set; } = null!;
        public virtual ReferenceUserRole Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
