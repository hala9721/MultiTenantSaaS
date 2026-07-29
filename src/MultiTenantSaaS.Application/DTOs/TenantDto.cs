using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SubscriptionPlan { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTenantRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SubscriptionPlan { get; set; }
    }

    public class UpdateTenantRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SubscriptionPlan { get; set; }
        public bool IsActive { get; set; }
    }
}
