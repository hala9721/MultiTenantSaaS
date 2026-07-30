using MultiTenantSaaS.Domain.Abstractions;

namespace MultiTenantSaaS.Domain.Entities
{
    public class ActivityLog : Entity
    {
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; } = null!; // "ProjectCreated", "TaskStatusChanged", etc
        public string EntityType { get; set; } = null!; // "Project", "Task", "User"
        public Guid EntityId { get; set; }
        public string? Details { get; set; } // JSON string for additional info

        // Relationships
        public Tenant Tenant { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
