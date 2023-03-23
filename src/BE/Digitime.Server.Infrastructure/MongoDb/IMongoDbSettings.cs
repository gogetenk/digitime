namespace Digitime.Server.Infrastructure.MongoDb;

public interface IMongoDbSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
    string ProjectsCollectionName { get; set; }
    string WorkspacesCollectionName { get; set; }
    string TimesheetsCollectionName { get; set; }
    string UsersCollectionName { get; set; }
}
