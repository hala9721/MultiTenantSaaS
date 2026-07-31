using MultiTenantSaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.DTOs;


public class TaskDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Domain.Enums.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public string? AssignedToUserName { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string CreatedByUserName { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TaskDetailDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Domain.Enums.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public UserDto? AssignedToUser { get; set; }
    public UserDto CreatedByUser { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTaskRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public Guid? AssignedToUserId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskPriority Priority { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class ChangeTaskStatusRequest
{
    public Domain.Enums.TaskStatus Status { get; set; }
}
