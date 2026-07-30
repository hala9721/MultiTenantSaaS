using MultiTenantSaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<Tenant?> GetTenantBySlugAsync(string slug);
        Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeTenantId = null);
    }
}
