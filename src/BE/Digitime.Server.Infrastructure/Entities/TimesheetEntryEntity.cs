using Digitime.Server.Domain.Models;
using Digitime.Server.Infrastructure.Mapping;

namespace Digitime.Server.Infrastructure.Entities;
public class TimesheetEntryEntity : IMapTo<TimesheetEntry>
{
    public string ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
