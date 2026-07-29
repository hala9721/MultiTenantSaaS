using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class RolePermission
    {

        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        // Relationships
        public Role Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
