using System.Collections.Generic;
using System.Threading.Tasks;
using Digitime.Server.Domain.Projects;

namespace Digitime.Server.Application.Abstractions;

public interface IProjectRepository
{
    Task<Project> FindByIdAsync(string id);
    Task<List<Project>> GetProjectsByReviewerId(string reviewerId);
    Task<List<Project>> GetProjectsByUserId(string userId);
}
