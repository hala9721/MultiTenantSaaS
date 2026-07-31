using MultiTenantSaaS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId);
        Task<ProjectDetailDto> CreateProjectAsync(CreateProjectRequest request);
        Task UpdateProjectAsync(Guid projectId, UpdateProjectRequest request);
        Task DeleteProjectAsync(Guid projectId);
        Task AddMemberToProjectAsync(Guid projectId, AddProjectMemberRequest request);
        Task RemoveMemberFromProjectAsync(Guid projectId, Guid userId);
    }
}
