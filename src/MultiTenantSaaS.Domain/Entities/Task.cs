using MultiTenantSaaS.Domain.Abstractions;
using MultiTenantSaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Entities
{
    public class Task:Entity
    {
        public Guid ProjectId { get; set; }
        public Guid TenantId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }

        // Relationships
        public Project Project { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
        public User? AssignedToUser { get; set; }
    }
}
