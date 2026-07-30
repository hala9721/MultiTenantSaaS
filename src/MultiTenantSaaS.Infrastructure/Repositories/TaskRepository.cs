
using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Domain.Enums;
using MultiTenantSaaS.Infrastructure.Data;


namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Entities.Task>, ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId, Guid tenantId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.TenantId == tenantId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByAssignedUserAsync(Guid userId, Guid tenantId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedToUserId == userId && t.TenantId == tenantId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(Domain.Enums.TaskStatus status, Guid tenantId)
        {
            return await _context.Tasks
                .Where(t => t.Status == status && t.TenantId == tenantId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Task?> GetTaskByIdAndTenantAsync(Guid taskId, Guid tenantId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.TenantId == tenantId);
        }
    }
}
