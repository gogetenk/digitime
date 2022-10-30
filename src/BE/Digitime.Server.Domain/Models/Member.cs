using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;
public class Member : ValueObject
{
    public Guid UserId { get; private set; }
    public MemberRoleEnum MemberRole { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
