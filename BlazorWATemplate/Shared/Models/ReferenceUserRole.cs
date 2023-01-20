using System;
using System.Collections.Generic;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class ReferenceUserRole
    {
        public ReferenceUserRole()
        {
            LinkUserRoles = new HashSet<LinkUserRole>();
        }

        public int RoleId { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<LinkUserRole> LinkUserRoles { get; set; }
    }
}
