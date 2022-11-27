using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Settings;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
}