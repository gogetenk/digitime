using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;

public record ProjectDto(string Id, string Title, string Description, string Code, string WorkspaceId, List<MemberDto> Members)
{
    public static implicit operator ProjectDto(Project project) =>
        new(project.Id, project.Title, project.Description, project.Code, project.WorkspaceId, project.Members.Select(x => (MemberDto)x).ToList());
}
