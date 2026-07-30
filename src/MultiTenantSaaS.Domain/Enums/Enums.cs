using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Domain.Enums
{
    public enum ProjectStatus
    {
        Active = 0,
        Archived = 1
    }

    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }

    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    public enum ProjectMemberRole
    {
        Owner = 0,
        Member = 1,
        Viewer = 2
    }
}
