using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public class ProjectMemberRepository : GenericRepository<ProjectMember>, IProjectMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectMemberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProjectMember?> GetMemberAsync(Guid projectId, Guid userId, Guid tenantId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId && m.TenantId == tenantId);
        }

        public async Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(Guid projectId, Guid tenantId)
        {
            return await _context.ProjectMembers
                .Where(m => m.ProjectId == projectId && m.TenantId == tenantId)
                .ToListAsync();
        }
    }
}
