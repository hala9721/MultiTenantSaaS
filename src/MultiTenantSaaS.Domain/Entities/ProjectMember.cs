using MultiTenantSaaS.Domain.Abstractions;
using MultiTenantSaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public ProjectMemberRole Role { get; set; } = ProjectMemberRole.Member;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public Project Project { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
