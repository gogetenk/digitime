using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Workspaces;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public class WorkspaceRepository : MongoRepository<WorkspaceEntity>, IWorkspaceRepository
{
    public WorkspaceRepository(IMongoDbSettings settings) : base(settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        Collection = database.GetCollection<WorkspaceEntity>(settings.WorkspacesCollectionName);
    }

    public async Task<Workspace> GetbyIdAsync(string id)
        => (await base.FindByIdAsync(id)).Adapt<Workspace>();


    public async Task<Workspace> Insert(Workspace workspace)
    {
        var workspaceEntity = workspace.Adapt<WorkspaceEntity>();
        await base.InsertOneAsync(workspaceEntity);
        var createdItem = await Collection.Find(x => x.Id == workspaceEntity.Id).FirstOrDefaultAsync();
        return createdItem.Adapt<Workspace>();
    }

    public async Task<Workspace> UpdateAsync(Workspace workspace)
    {
        var workspaceEntity = workspace.Adapt<WorkspaceEntity>();
        await base.ReplaceOneAsync(workspaceEntity);
        var updatedItem = await Collection.Find(x => x.Id == workspaceEntity.Id).FirstOrDefaultAsync();
        return updatedItem.Adapt<Workspace>();
    }
}
