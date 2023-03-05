using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("timesheets")]
public class TimesheetEntity : EntityBase
{
    public List<TimesheetEntryEntity> TimesheetEntries { get; set; }
    public Worker Worker { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}

public enum TimesheetStatusEnum
{
    Draft,
    Submitted,
    Approved,
    Rejected
}
