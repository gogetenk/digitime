using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("workspaces")]
public class WorkspaceEntity : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<WorkspaceMemberEntity> Members { get; set; }
    public SubscriptionEntity Subscription { get; private set; }
}

public enum WorkspaceMemberRoleEntityEnum
{
    Worker,
    Reviewer,
    ProjectAdmin,
    WorkspaceAdmin,
    Pending
}

public record WorkspaceMemberEntity(string UserId, string Fullname, string Email, string ProfilePicture, WorkspaceMemberRoleEntityEnum MemberRole);
public record WorkspaceProjectEntity(string Id, string Title, string Code);