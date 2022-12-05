using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.Helpers;

namespace Digitime.Server.Infrastructure.Entities;
public class TimesheetEntryEntity : EntityBase
{
    public DateTime Date { get; private set; }
    public float Hours { get; private set; }
    public Project Project { get; private set; }
    public List<ReviewerEntity> Reviewers { get; private set; }
    public TimesheetStatus Status { get; private set; }

    public static implicit operator TimesheetEntryEntity(TimesheetEntry timesheetEntry)
        => new TimesheetEntryEntity
        {
            Id = timesheetEntry.Id.ToObjectId(),
            Date = timesheetEntry.Date,
            Hours = timesheetEntry.Hours,
            Project = timesheetEntry.Project,
            Reviewers = timesheetEntry.Reviewers.Select(x => (ReviewerEntity)x).ToList(),
            Status = timesheetEntry.Status
        };

    public static implicit operator TimesheetEntry(TimesheetEntryEntity timesheetEntryEntity)
        => TimesheetEntry.Create(
            timesheetEntryEntity.Id.ToString(),
            timesheetEntryEntity.Date,
            timesheetEntryEntity.Hours,
            timesheetEntryEntity.Project,
            timesheetEntryEntity.Reviewers.Select(x => (Reviewer)x).ToList(),
            timesheetEntryEntity.Status);
}

public record ReviewerEntity(string UserId, string Fullname, string Email)
{
    public static implicit operator ReviewerEntity(Reviewer reviewer)
        => new ReviewerEntity(reviewer.UserId, reviewer.Fullname, reviewer.Email);

    public static implicit operator Reviewer(ReviewerEntity reviewerEntity)
        => new Reviewer(reviewerEntity.UserId, reviewerEntity.Fullname, reviewerEntity.Email);
}