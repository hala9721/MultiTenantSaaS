using MultiTenantSaaS.Domain.Abstractions;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Tenant : Entity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!; // للـ URL: example.com/tenant-slug
        public string Email { get; set; } = null!;
        public string? SubscriptionPlan { get; set; } // "free", "pro", "enterprise"
        public bool IsActive { get; set; } = true;
        public int MaxUsers { get; set; } = 5;

        // Relationships
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();


    }
}
