using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("users")]
public class UserEntity : EntityBase
{
    public string ExternalId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
}
