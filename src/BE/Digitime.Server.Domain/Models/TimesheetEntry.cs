using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;
public class TimesheetEntry : ValueObject
{
    public string ProjectId { get; private set; }
    public string ProjectTitle { get; private set; }
    public DateTime Date { get; private set; }
    public int Hours { get; private set; }

    
    public TimesheetEntry(string projectId, string projectTitle, DateTime date, int hours)
    {
        Date = date;
        Hours = hours;
        ProjectId = projectId;
        ProjectTitle = projectTitle;
    }

    public static TimesheetEntry Create(string projectId, string projectTitle, DateTime date, int hours)
        => new(projectId, projectTitle, date, hours);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProjectId;
        yield return ProjectTitle;
        yield return Date;
        yield return Hours;
    }
}
