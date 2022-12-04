using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;

public record TimesheetEntryDto(DateTime Date, int Hours, TimesheetEntryProjectDto Project)
{
    public static implicit operator TimesheetEntryDto(TimesheetEntry domainObject) =>
        new(domainObject.Date,
            domainObject.Hours,
            domainObject.Project);

    public static implicit operator TimesheetEntry(TimesheetEntryDto dto) =>
        new(dto.Date,
            dto.Hours,
            dto.Project);
}
