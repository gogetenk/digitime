using Digitime.Shared.Dto;

namespace Digitime.Client.Infrastructure.Abstractions;

public interface IDataStore
{
    Task SynchronizeData();
    Task<CalendarDto> GetCalendar(DateTime date, string country);
}
