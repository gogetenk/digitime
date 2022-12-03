using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;

public record TimesheetEntryDto(string ProjectId, string ProjectTitle, DateTime Date, int Hours)
{
    public static implicit operator TimesheetEntryDto(TimesheetEntry domainObject) =>
        new(domainObject.ProjectId,
            domainObject.ProjectTitle,
            domainObject.Date,
            domainObject.Hours);

    public static implicit operator TimesheetEntry(TimesheetEntryDto dto) =>
        new(dto.ProjectId,
            dto.ProjectTitle,
            dto.Date,
            dto.Hours);
}
