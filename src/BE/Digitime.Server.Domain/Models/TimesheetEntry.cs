using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;

public record TimesheetEntry // ValueObject
{
    public TimesheetEntryProject Project { get; init; }
    public DateTime Date { get; init; }
    public int Hours { get; init; }

    public TimesheetEntry(DateTime date, int hours, TimesheetEntryProject project)
    {
        Date = date;
        Hours = hours;
        Project = project;
    }

    public static TimesheetEntry Create(DateTime date, int hours, TimesheetEntryProject project)
        => new(date, hours, project);

    //protected override IEnumerable<object> GetEqualityComponents()
    //{
    //    yield return ProjectId;
    //    yield return ProjectTitle;
    //    yield return Date;
    //    yield return Hours;
    //}
}


