namespace MultiTenantSaaS.Application.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }
    ITenantRepository Tenants { get; }
    IActivityLogRepository ActivityLogs { get; }
    IProjectMemberRepository ProjectMembers { get; }

    Task<int> SaveChangesAsync();
}
