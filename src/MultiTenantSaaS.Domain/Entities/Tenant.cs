using MultiTenantSaaS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Tenant :Entity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!; // للـ URL: example.com/tenant-slug
        public string Email { get; set; } = null!;
        public string? SubscriptionPlan { get; set; } // "free", "pro", "enterprise"
        public bool IsActive { get; set; } = true;

        // Relationships
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
