namespace Digitime.Server.Domain.Models;

internal class User : Entity<string>
{
    public string Lastname { get; private set; }
    public string Firstname { get; private set; }
    public string Email { get; private set; }

    public User(string id, string lastname, string firstname) : base(id)
    {
        Lastname = lastname;
        Firstname = firstname;
    }
}
