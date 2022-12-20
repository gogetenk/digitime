using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public class TimesheetRepository : MongoRepository<TimesheetEntity>, ITimesheetRepository
{
    public TimesheetRepository(IMongoDbSettings settings) : base(settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        Collection = database.GetCollection<TimesheetEntity>(settings.TimesheetsCollectionName);
    }

    public async Task<Timesheet> CreateAsync(Timesheet timesheet)
    {
        var entity = timesheet.Adapt<TimesheetEntity>();
        await Collection.InsertOneAsync(entity);
        var createdItem = await Collection.Find(x => x.Id == entity.Id).FirstOrDefaultAsync();
        return createdItem.Adapt<Timesheet>();
    }

    public Task DeleteAsync(string id)
    {
        return Collection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<Timesheet> GetbyIdAsync(string id)
    {
        return (await Collection.Find(x => x.Id == id).SingleOrDefaultAsync()).Adapt<Timesheet>();
    }

    public async Task<List<Timesheet>> GetbyUserAndMonthOfyear(string userId, int month, int year)
    {
        var beginDate = new DateTime(year, month, 1);
        var endDate = beginDate.AddMonths(1).AddDays(-1);
        return (await FilterByAsync(x => 
            x.Worker.UserId == userId &&
            x.CreateDate >= beginDate &&
            x.CreateDate <= endDate))
        .Adapt<List<Timesheet>>();
    }

    public async Task<List<Timesheet>> GetTimesheetsFromProjectIds(string[] projectIds)
    {
        var timesheets = await FilterByAsync(x => x.TimesheetEntries.Any(y => projectIds.Contains(y.Project.Id)));
        return timesheets.Adapt<List<Timesheet>>();
    }

    public Task UpdateAsync(Timesheet timesheet)
    {
        var entity = timesheet.Adapt<TimesheetEntity>();
        var filter = Builders<TimesheetEntity>.Filter.Eq(doc => doc.Id, entity.Id);
        return Collection.FindOneAndReplaceAsync(filter, entity);
    }
}
