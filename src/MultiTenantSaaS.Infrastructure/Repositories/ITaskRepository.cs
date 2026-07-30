

using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Domain.Enums;
namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public interface ITaskRepository : IGenericRepository<Domain.Entities.Task>
    {
        Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId, Guid tenantId);
        Task<IEnumerable<Domain.Entities.Task>> GetTasksByAssignedUserAsync(Guid userId, Guid tenantId);
        Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(Domain.Enums.TaskStatus status, Guid tenantId);
        Task<Domain.Entities.Task?> GetTaskByIdAndTenantAsync(Guid taskId, Guid tenantId);
    }
}
