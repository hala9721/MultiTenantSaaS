using MultiTenantSaaS.Domain.Abstractions;
using MultiTenantSaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Project:Entity
    {
        public Guid TenantId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        // Relationships
        public Tenant Tenant { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}
