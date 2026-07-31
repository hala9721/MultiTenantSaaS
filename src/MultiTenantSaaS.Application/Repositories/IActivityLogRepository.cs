using MultiTenantSaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        Task<IEnumerable<ActivityLog>> GetActivitiesByTenantAsync(Guid tenantId, int take = 100);
        Task<IEnumerable<ActivityLog>> GetActivitiesByUserAsync(Guid userId, Guid tenantId, int take = 50);
    }
}
