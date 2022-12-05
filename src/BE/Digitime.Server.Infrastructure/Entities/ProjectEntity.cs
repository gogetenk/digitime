using Digitime.Server.Domain.Projects.ValueObjects;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.MongoDb;
using MongoDB.Bson;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("projects")]
public class ProjectEntity : EntityBase
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string WorkspaceId { get; set; }
    public List<ProjectMemberEntity> Members { get; set; }

    public static implicit operator ProjectEntity(Domain.Timesheets.ValueObjects.Project project) =>
        new()
        {
            Id = ObjectId.Parse(project.Id),
            Title = project.Title,
            Code = project.Code,
        };

    public static implicit operator Domain.Timesheets.ValueObjects.Project(ProjectEntity projectEntity) =>
        new(projectEntity.Id.ToString(), projectEntity.Title, projectEntity.Code);
}

public enum MemberRoleEntityEnum
{
    Worker,
    Reviewer
}

public record ProjectMemberEntity(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEntityEnum MemberRole)
{
    public static implicit operator ProjectMember(ProjectMemberEntity projectMemberEntity) =>
        new(projectMemberEntity.UserId, projectMemberEntity.Fullname, projectMemberEntity.Email, projectMemberEntity.ProfilePicture, (MemberRoleEnum)projectMemberEntity.MemberRole);

    public static implicit operator ProjectMemberEntity(ProjectMember projectMember) =>
        new(projectMember.UserId, projectMember.Fullname, projectMember.Email, projectMember.ProfilePicture, (MemberRoleEntityEnum)projectMember.MemberRole);

    public static implicit operator Reviewer(ProjectMemberEntity projectMember) =>
        new(projectMember.UserId, projectMember.Fullname, projectMember.Email);
}
