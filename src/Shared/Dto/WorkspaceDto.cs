using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;
public record WorkspaceDto(string Id, string Title, string Description, List<MemberDto> Members)
{
    public static implicit operator WorkspaceDto(Workspace workspace) =>
        new(workspace.Id, workspace.Title, workspace.Description, workspace.Members.Select(x => (MemberDto)x).ToList());
}
