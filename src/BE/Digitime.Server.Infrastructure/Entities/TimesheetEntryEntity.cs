using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.MongoDb;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitime.Server.Infrastructure.Entities;
public class TimesheetEntryEntity : EntityBase
{
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Date { get; private set; }
    public float Hours { get; private set; }
    public Project Project { get; private set; }
    public TimesheetStatus Status { get; private set; }
}
