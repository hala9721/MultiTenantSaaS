using MultiTenantSaaS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MultiTenantSaaS.Application.Abstractions.Repositories;

    public interface IProjectMemberRepository : IGenericRepository<ProjectMember>
    {
        Task<ProjectMember?> GetMemberAsync(Guid projectId, Guid userId, Guid tenantId);
        Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(Guid projectId, Guid tenantId);
    }

