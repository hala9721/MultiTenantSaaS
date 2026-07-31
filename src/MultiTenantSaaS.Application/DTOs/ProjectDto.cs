
using MultiTenantSaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.DTOs;


public class ProjectDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProjectStatus Status { get; set; }
    public string CreatedByUserName { get; set; } = null!;
    public int TaskCount { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProjectDetailDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProjectStatus Status { get; set; }
    public UserDto CreatedByUser { get; set; } = null!;
    public List<TaskDto> Tasks { get; set; } = new();
    public List<ProjectMemberDto> Members { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateProjectRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
}

public class UpdateProjectRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ProjectStatus Status { get; set; }
}

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime AddedAt { get; set; }
}

public class AddProjectMemberRequest
{
    public Guid UserId { get; set; }
    public string Role { get; set; } = null!; // "Owner", "Member", "Viewer"
}