using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Settings;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
    public string ProjectsCollectionName { get; set; }
    public string TimesheetsCollectionName { get; set; }
    public string UsersCollectionName { get; set; }
    public string WorkspacesCollectionName { get; set; }
}