using MultiTenantSaaS.Domain.Entities;
namespace MultiTenantSaaS.Application.Abstractions.Repositories;

    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAndTenantAsync(string email, Guid tenantId);
        Task<User?> GetUserByIdAndTenantAsync(Guid userId, Guid tenantId);
        Task<IEnumerable<User>> GetUsersByTenantAsync(Guid tenantId);
    }

