using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;
public class TimesheetEntry : ValueObject
{
    public Guid ProjectId { get; private set; }
    public string ProjectTitle { get; private set; }
    public DateTime Date { get; private set; }
    public int Hours { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
