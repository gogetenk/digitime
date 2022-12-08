using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.Users;

public class User : AggregateRoot<string>
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Email { get; init; }
    public string ProfilePicture { get; init; }
    public string ExternalId { get; init; }

    public User(string id, string firstname, string lastname, string email, string profilePicture, string externalId)
        : base(id)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        ProfilePicture = profilePicture;
        ExternalId = externalId;
    }
}
