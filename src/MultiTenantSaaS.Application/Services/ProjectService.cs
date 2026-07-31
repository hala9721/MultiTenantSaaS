namespace MultiTenantSaaS.Application.Services;

using Microsoft.Extensions.Logging;
using MultiTenantSaaS.Application.Abstractions.Repositories;
using MultiTenantSaaS.Application.DTOs;
using MultiTenantSaaS.Application.Exceptions;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Domain.Enums;



public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMultiTenantContext _tenantContext;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(
        IUnitOfWork unitOfWork,
        IMultiTenantContext tenantContext,
        ILogger<ProjectService> logger)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var projects = await _unitOfWork.Projects.GetProjectsByTenantAsync(tenantId);

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            TenantId = p.TenantId,
            Name = p.Name,
            Description = p.Description,
            Slug = p.Slug,
            Status = p.Status,
            CreatedByUserName = $"{p.CreatedByUser.FirstName} {p.CreatedByUser.LastName}",
            TaskCount = p.Tasks?.Count ?? 0,
            MemberCount = p.Members?.Count ?? 0,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }

    public async Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId)
    {
        var tenantId = _tenantContext.CurrentTenantId;

        var project = await _unitOfWork.Projects.GetProjectWithMembersAsync(projectId, tenantId);

        if (project == null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found");
        }

        var projectWithTasks = await _unitOfWork.Projects.GetProjectWithTasksAsync(projectId, tenantId);

        return new ProjectDetailDto
        {
            Id = project.Id,
            TenantId = project.TenantId,
            Name = project.Name,
            Description = project.Description,
            Slug = project.Slug,
            Status = project.Status,
            CreatedByUser = new UserDto
            {
                Id = project.CreatedByUser.Id,
                FirstName = project.CreatedByUser.FirstName,
                LastName = project.CreatedByUser.LastName,
                Email = project.CreatedByUser.Email
            },
            Tasks = projectWithTasks?.Tasks?.Select(t => new TaskDto
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                TenantId = t.TenantId,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                AssignedToUserName = t.AssignedToUser != null
                    ? $"{t.AssignedToUser.FirstName} {t.AssignedToUser.LastName}"
                    : null,
                AssignedToUserId = t.AssignedToUserId,
                CreatedByUserName = $"{t.CreatedByUser.FirstName} {t.CreatedByUser.LastName}",
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList() ?? new(),
            Members = project.Members.Select(m => new ProjectMemberDto
            {
                Id = m.Id,
                UserId = m.UserId,
                UserName = $"{m.User.FirstName} {m.User.LastName}",
                UserEmail = m.User.Email,
                Role = m.Role.ToString(),
                AddedAt = m.AddedAt
            }).ToList(),
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<ProjectDetailDto> CreateProjectAsync(CreateProjectRequest request)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var userId = _tenantContext.CurrentUserId;

        // تحقق من الـ slug
        var existingProject = await _unitOfWork.Projects
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Slug == request.Slug);

        if (existingProject != null)
        {
            throw new BadRequestException($"Project with slug '{request.Slug}' already exists");
        }

        var project = new Project
        {
            TenantId = tenantId,
            CreatedByUserId = userId,
            Name = request.Name,
            Description = request.Description,
            Slug = request.Slug,
            Status = ProjectStatus.Active
        };

        await _unitOfWork.Projects.AddAsync(project);

        // Add the creator as owner
        var member = new ProjectMember
        {
            ProjectId = project.Id,
            UserId = userId,
            TenantId = tenantId,
            Role = ProjectMemberRole.Owner
        };

        await _unitOfWork.ProjectMembers.AddAsync(member);

        // Log activity
        var activity = new ActivityLog
        {
            TenantId = tenantId,
            UserId = userId,
            Action = "ProjectCreated",
            EntityType = "Project",
            EntityId = project.Id
        };

        await _unitOfWork.ActivityLogs.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Project '{ProjectName}' created by user {UserId}", project.Name, userId);

        return await GetProjectByIdAsync(project.Id);
    }

    public async System.Threading.Tasks.Task UpdateProjectAsync(Guid projectId, UpdateProjectRequest request)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var userId = _tenantContext.CurrentUserId;

        var project = await _unitOfWork.Projects.GetProjectByIdAndTenantAsync(projectId, tenantId);

        if (project == null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found");
        }

        project.Name = request.Name;
        project.Description = request.Description;
        project.Status = request.Status;
        project.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        // Log activity
        var activity = new ActivityLog
        {
            TenantId = tenantId,
            UserId = userId,
            Action = "ProjectUpdated",
            EntityType = "Project",
            EntityId = project.Id
        };

        await _unitOfWork.ActivityLogs.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation($"Project '{project.Name}' updated by user {userId}");
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(Guid projectId)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var userId = _tenantContext.CurrentUserId;

        var project = await _unitOfWork.Projects.GetProjectByIdAndTenantAsync(projectId, tenantId);

        if (project == null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found");
        }

        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.SaveChangesAsync();

        // Log activity
        var activity = new ActivityLog
        {
            TenantId = tenantId,
            UserId = userId,
            Action = "ProjectDeleted",
            EntityType = "Project",
            EntityId = project.Id
        };

        await _unitOfWork.ActivityLogs.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation($"Project '{project.Name}' deleted by user {userId}");
    }

    public async System.Threading.Tasks.Task AddMemberToProjectAsync(Guid projectId, AddProjectMemberRequest request)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var userId = _tenantContext.CurrentUserId;

        var project = await _unitOfWork.Projects.GetProjectByIdAndTenantAsync(projectId, tenantId);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found");
        }

        var user = await _unitOfWork.Users.GetUserByIdAndTenantAsync(request.UserId, tenantId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found");
        }

        if (!Enum.TryParse<ProjectMemberRole>(request.Role, out var role))
        {
            throw new BadRequestException($"Invalid role: {request.Role}");
        }

        var existingMember = await _unitOfWork.ProjectMembers
            .GetMemberAsync(projectId, request.UserId, tenantId);

        if (existingMember != null)
        {
            throw new BadRequestException("User is already a member of this project");
        }

        var member = new ProjectMember
        {
            ProjectId = projectId,
            UserId = request.UserId,
            TenantId = tenantId,
            Role = role
        };

        await _unitOfWork.ProjectMembers.AddAsync(member);

        var activity = new ActivityLog
        {
            TenantId = tenantId,
            UserId = userId,
            Action = "ProjectMemberAdded",
            EntityType = "ProjectMember",
            EntityId = project.Id,
            Details = request.UserId.ToString()
        };

        await _unitOfWork.ActivityLogs.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {RequestUserId} added to project {ProjectId}", request.UserId, projectId);
    }

    public async System.Threading.Tasks.Task RemoveMemberFromProjectAsync(Guid projectId, Guid userId)
    {
        var tenantId = _tenantContext.CurrentTenantId;
        var currentUserId = _tenantContext.CurrentUserId;

        var project = await _unitOfWork.Projects.GetProjectByIdAndTenantAsync(projectId, tenantId);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found");
        }

        var member = await _unitOfWork.ProjectMembers.GetMemberAsync(projectId, userId, tenantId);
        if (member == null)
        {
            throw new NotFoundException($"User {userId} is not a member of this project");
        }

        _unitOfWork.ProjectMembers.Delete(member);

        var activity = new ActivityLog
        {
            TenantId = tenantId,
            UserId = currentUserId,
            Action = "ProjectMemberRemoved",
            EntityType = "ProjectMember",
            EntityId = project.Id,
            Details = userId.ToString()
        };

        await _unitOfWork.ActivityLogs.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} removed from project {ProjectId}", userId, projectId);
    }
}

