using MultiTenantSaaS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class User: Entity
    {
        public Guid TenantId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        // Relationships
        public Tenant Tenant { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    }
}
