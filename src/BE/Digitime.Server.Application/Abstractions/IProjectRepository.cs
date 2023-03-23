using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Projects.ValueObjects;

namespace Digitime.Server.Application.Abstractions;

public interface IProjectRepository
{
    Task<Project> FindByIdAsync(string id);
    Task<List<Project>> GetProjectsByReviewerId(string reviewerId);
    Task<List<Project>> GetProjectsByUserId(string userId);
    Task<Project> InsertOneAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<ProjectMember> FindMemberByEmailAsync(string email);
}
