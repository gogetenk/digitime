using Digitime.Server.Domain.Models;
using Digitime.Server.Mapping;

namespace Digitime.Server.Dto;

public class TimesheetEntryDto : IMapFrom<Timesheet>
{
    public Guid ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
