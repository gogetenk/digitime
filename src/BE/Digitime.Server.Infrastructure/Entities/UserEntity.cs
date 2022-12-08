using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("users")]
public class UserEntity : EntityBase
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Email { get; init; }
    public string ProfilePicture { get; init; }
    public string ExternalId { get; init; }
}
