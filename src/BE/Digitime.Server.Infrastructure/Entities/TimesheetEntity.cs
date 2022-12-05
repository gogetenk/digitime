using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.MongoDb;
using MongoDB.Bson;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("timesheets")]
public class TimesheetEntity : EntityBase
{
    public List<TimesheetEntryEntity> TimesheetEntries { get; set; }
    public Worker Worker { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    public static implicit operator TimesheetEntity(Timesheet timesheet) =>
        new()
        {
            Id = ObjectId.Parse(timesheet.Id),
            TimesheetEntries = timesheet.TimesheetEntries.Select(x => (TimesheetEntryEntity)x).ToList(),
            Worker = timesheet.Worker,
            CreateDate = timesheet.CreateDate,
            UpdateDate = timesheet.UpdateDate
        };

    public static implicit operator Timesheet(TimesheetEntity timesheetEntity) =>
        timesheetEntity is not null ?
            Timesheet.Create(
                timesheetEntity.Id.ToString(),
                timesheetEntity.Worker,
                timesheetEntity.UpdateDate,
                timesheetEntity.CreateDate,
                timesheetEntity.TimesheetEntries.Select(x => (TimesheetEntry)x).ToList())
        : null;
}

public enum TimesheetStatusEnum
{
    Draft,
    Submitted,
    Approved,
    Rejected
}

public record WorkerEntity(string UserId, string FirstName, string LastName, string Email, string ProfilePicture)
{
    public static implicit operator Worker(WorkerEntity workerEntity) =>
        new(workerEntity.UserId, workerEntity.FirstName, workerEntity.LastName, workerEntity.Email, workerEntity.ProfilePicture);

    public static implicit operator WorkerEntity(Worker worker) =>
        new(worker.UserId, worker.FirstName, worker.LastName, worker.Email, worker.ProfilePicture);
}
