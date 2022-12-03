using Digitime.Server.Domain.Models;
using Digitime.Server.Infrastructure.Helpers;
using Digitime.Server.Infrastructure.MongoDb;
using MongoDB.Bson;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("timesheets")]
public class TimesheetEntity : EntityBase
{
    public List<TimesheetEntryEntity> TimesheetEntries { get; init; } = new();
    public DateTime BeginDate { get; init; }
    public DateTime EndDate { get; init; }
    public TimeSpan Period { get; init; }
    public int Hours { get; init; }
    public TimesheetStatusEnum Status { get; init; }
    public string CreatorId { get; init; }
    public string ApproverId { get; init; }
    public DateTime? ApproveDate { get; init; }
    public DateTime? CreateDate { get; init; }
    public DateTime? UpdateDate { get; init; }

    public static implicit operator TimesheetEntity(Timesheet timesheet)
        => new TimesheetEntity
        {
            Id = timesheet.Id?.ToObjectId() ?? ObjectId.Empty,
            TimesheetEntries = timesheet.TimesheetEntries.Select(x => (TimesheetEntryEntity)x).ToList(),
            BeginDate = timesheet.BeginDate,
            EndDate = timesheet.EndDate,
            Period = timesheet.Period,
            Hours = timesheet.Hours,
            Status = (TimesheetStatusEnum)timesheet.Status,
            CreatorId = timesheet.CreatorId,
            ApproverId = timesheet.ApproverId,
            ApproveDate = timesheet.ApproveDate,
            CreateDate = timesheet.CreateDate,
            UpdateDate = timesheet.UpdateDate
        };

    public static implicit operator Timesheet(TimesheetEntity timesheet)
        => Timesheet.Create(
                            timesheet.BeginDate,
                            timesheet.CreatorId,
                            timesheet.Id.ToString() ?? null,
                            timesheet.ApproverId,
                            timesheet.ApproveDate,
                            timesheet.TimesheetEntries.Select(x => (TimesheetEntry)x).ToList(),
                            timesheet.EndDate,
                            timesheet.Period,
                            timesheet.Hours,
                            timesheet.CreateDate,
                            timesheet.UpdateDate);
}

public enum TimesheetStatusEnum
{
    Draft = 0,
    Submitted = 1,
    Approved = 2
}
