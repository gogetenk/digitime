using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("users")]
public class UserEntity : EntityBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string ExternalId { get; set; }
}
