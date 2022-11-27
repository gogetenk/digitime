using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digitime.Server.Infrastructure.Entities;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public interface IMongoDbContext
{
    IMongoCollection<TimesheetEntryEntity> TimesheetEntries { get; }
    IMongoCollection<UserEntity> Users { get; }
}
