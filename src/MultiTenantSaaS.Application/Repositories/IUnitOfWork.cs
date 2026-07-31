using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Infrastructure.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        IProjectRepository Projects { get; }
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        ITenantRepository Tenants { get; }
        IActivityLogRepository ActivityLogs { get; }
        IProjectMemberRepository ProjectMembers { get; }

        Task<int> SaveChangesAsync();
    }
}
