using Digitime.Server.Infrastructure.Entities;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public interface IMongoDbContext
{
    IMongoCollection<TimesheetEntryEntity> TimesheetEntries { get; }
    IMongoCollection<UserEntity> Users { get; }
}
