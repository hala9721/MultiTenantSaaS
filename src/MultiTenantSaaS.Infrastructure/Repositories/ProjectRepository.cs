namespace MultiTenantSaaS.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;


public class ProjectRepository : GenericRepository<Project>, IProjectRepository

{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetProjectsByTenantAsync(Guid tenantId)
    {
        return await _context.Projects
            .Where(p => p.TenantId == tenantId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAndTenantAsync(Guid projectId, Guid tenantId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.TenantId == tenantId);
    }

    public async Task<Project?> GetProjectWithTasksAsync(Guid projectId, Guid tenantId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.TenantId == tenantId);
    }

    public async Task<Project?> GetProjectWithMembersAsync(Guid projectId, Guid tenantId)
    {
        return await _context.Projects
            .Include(p => p.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.TenantId == tenantId);
    }
}