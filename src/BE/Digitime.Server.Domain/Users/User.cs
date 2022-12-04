using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.Users;

public class User : AggregateRoot<string>
{
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Email { get; private set; }
    public string ProfilePicture { get; private set; }
    public string ExternalId { get; private set; }

    private User(string id, string firstname, string lastname, string email, string profilePicture, string externalId) : base(id)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        ProfilePicture = profilePicture;
        ExternalId = externalId;
    }

    public static User Create(string id, string firstname, string lastname, string email, string profilePicture, string externalId)
        => new User(id, firstname, lastname, email, profilePicture, externalId);
}
