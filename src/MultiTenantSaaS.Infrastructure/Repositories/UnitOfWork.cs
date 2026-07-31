using MultiTenantSaaS.Application.Abstractions.Repositories;
using MultiTenantSaaS.Infrastructure.Data;


namespace MultiTenantSaaS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IProjectRepository? _projectRepository;
    private ITaskRepository? _taskRepository;
    private IUserRepository? _userRepository;
    private ITenantRepository? _tenantRepository;
    private IActivityLogRepository? _activityLogRepository;
    private IProjectMemberRepository? _projectMemberRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProjectRepository Projects
    {
        get => _projectRepository ??= new ProjectRepository(_context);
    }

    public ITaskRepository Tasks
    {
        get => _taskRepository ??= new TaskRepository(_context);
    }

    public IUserRepository Users
    {
        get => _userRepository ??= new UserRepository(_context);
    }

    public ITenantRepository Tenants
    {
        get => _tenantRepository ??= new TenantRepository(_context);
    }

    public IActivityLogRepository ActivityLogs
    {
        get => _activityLogRepository ??= new ActivityLogRepository(_context);
    }

    public IProjectMemberRepository ProjectMembers
    {
        get => _projectMemberRepository ??= new ProjectMemberRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
