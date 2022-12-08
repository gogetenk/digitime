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
}

public record ReviewerEntity(string UserId, string Fullname, string Email);