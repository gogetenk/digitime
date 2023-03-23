using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("projects")]
public class ProjectEntity : EntityBase
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string WorkspaceId { get; set; }
    public List<ProjectMemberEntity> Members { get; set; }
}

public enum MemberRoleEntityEnum
{
    Worker,
    Reviewer,
    ProjectAdmin,
    WorkspaceAdmin,
    Pending
}

public record ProjectMemberEntity(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEntityEnum MemberRole);