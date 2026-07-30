using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetTenantBySlugAsync(string slug)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Slug == slug);
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeTenantId = null)
        {
            var query = _context.Tenants.Where(t => t.Slug == slug);

            if (excludeTenantId.HasValue)
            {
                query = query.Where(t => t.Id != excludeTenantId.Value);
            }

            return !await query.AnyAsync();
        }

    }
}
