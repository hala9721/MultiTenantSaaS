using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {


        private readonly ApplicationDbContext _context;

        public ActivityLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActivityLog>> GetActivitiesByTenantAsync(Guid tenantId, int take = 100)
        {
            return await _context.ActivityLogs
                .Where(a => a.TenantId == tenantId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetActivitiesByUserAsync(Guid userId, Guid tenantId, int take = 50)
        {
            return await _context.ActivityLogs
                .Where(a => a.UserId == userId && a.TenantId == tenantId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(take)
                .ToListAsync();
        }
    }
}
