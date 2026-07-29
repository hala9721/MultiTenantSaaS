using MultiTenantSaaS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Role :Entity
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = null!; // "Admin", "Manager", "User"
        public string Description { get; set; } = null!;

        // Relationships
        public Tenant Tenant { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
