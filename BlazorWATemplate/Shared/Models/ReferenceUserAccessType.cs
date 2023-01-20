using System;
using System.Collections.Generic;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class ReferenceUserAccessType
    {
        public ReferenceUserAccessType()
        {
            LinkUserRoles = new HashSet<LinkUserRole>();
        }

        public int AccessTypeId { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<LinkUserRole> LinkUserRoles { get; set; }
    }
}
