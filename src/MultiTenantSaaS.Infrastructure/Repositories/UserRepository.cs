using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAndTenantAsync(string email, Guid tenantId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.TenantId == tenantId);
        }

        public async Task<User?> GetUserByIdAndTenantAsync(Guid userId, Guid tenantId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);
        }

        public async Task<IEnumerable<User>> GetUsersByTenantAsync(Guid tenantId)
        {
            return await _context.Users
                .Where(u => u.TenantId == tenantId)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }
    }
}
