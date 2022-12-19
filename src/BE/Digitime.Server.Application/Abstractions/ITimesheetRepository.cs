using System.Collections.Generic;
using System.Threading.Tasks;
using Digitime.Server.Domain.Timesheets;

namespace Digitime.Server.Application.Abstractions;

public interface ITimesheetRepository
{
    Task<Timesheet> CreateAsync(Timesheet timesheet);
    Task<Timesheet> GetbyIdAsync(string id);
    Task<List<Timesheet>> GetbyUserAndMonthOfyear(string userId, int month, int year);
    Task UpdateAsync(Timesheet timesheet);
    Task DeleteAsync(string id);
}
