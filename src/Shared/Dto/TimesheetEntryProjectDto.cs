using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;
public record TimesheetEntryProjectDto(string Id, string Title, string Code)
{
    public static implicit operator TimesheetEntryProjectDto(TimesheetEntryProject domainObject) =>
        new(domainObject.Id,
            domainObject.Title,
            domainObject.Code);

    public static implicit operator TimesheetEntryProject(TimesheetEntryProjectDto dto) =>
        new(dto.Id,
            dto.Title,
            dto.Code);
}