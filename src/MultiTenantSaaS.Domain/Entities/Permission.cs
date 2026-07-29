using MultiTenantSaaS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Permission:Entity
    {
        public string Name { get; set; } = null!; // "users.read", "users.create", "users.delete"
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!; // "users", "tenants", "reports"

        // Relationships
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
