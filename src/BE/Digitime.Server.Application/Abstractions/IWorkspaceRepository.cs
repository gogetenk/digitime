using System.Threading.Tasks;
using Digitime.Server.Domain.Workspaces;

namespace Digitime.Server.Application.Abstractions;

public interface IWorkspaceRepository
{
    Task<Workspace> GetbyIdAsync(string id);
    Task<Workspace> UpdateAsync(Workspace workspace);
    Task<Workspace> Insert(Workspace workspace);
}
