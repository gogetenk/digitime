using Digitime.Server.Domain.Models;

namespace Digitime.Server.Infrastructure.Entities;
public record TimesheetEntryProjectEntity(string Id, string Title, string Code)
{
    public static implicit operator TimesheetEntryProjectEntity(TimesheetEntryProject domainObject) =>
        new(domainObject.Id,
            domainObject.Title,
            domainObject.Code);

    public static implicit operator TimesheetEntryProject(TimesheetEntryProjectEntity entity) =>
        new(entity.Id,
            entity.Title,
            entity.Code);
}
