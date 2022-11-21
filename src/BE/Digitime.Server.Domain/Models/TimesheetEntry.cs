using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;
public class TimesheetEntry : ValueObject
{
    public TimesheetEntry(DateTime date, int hours, string projectId, string projectTitle)
    {
        Date = date;
        Hours = hours;
        ProjectId = projectId;
        ProjectTitle = projectTitle;
    }

    public string ProjectId { get; private set; }
    public string ProjectTitle { get; private set; }
    public DateTime Date { get; private set; }
    public int Hours { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProjectId;
        yield return Date;
    }
}
