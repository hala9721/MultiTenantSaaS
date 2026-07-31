using MultiTenantSaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Abstractions.Repositories;

    public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByTenantAsync(Guid tenantId);
    Task<Project?> GetProjectByIdAndTenantAsync(Guid projectId, Guid tenantId);
    Task<Project?> GetProjectWithTasksAsync(Guid projectId, Guid tenantId);
    Task<Project?> GetProjectWithMembersAsync(Guid projectId, Guid tenantId);
}
