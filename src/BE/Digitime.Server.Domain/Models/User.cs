using System;

namespace Digitime.Server.Domain.Models;
internal class User : Entity<Guid>
{

    public string Lastname { get; private set; }
    public string Firstname { get; private set; }
    public string Email { get; private set; }

    public User(Guid id, string lastname, string firstname) : base(id)
    {
        Lastname = lastname;
        Firstname = firstname;
    }

}
