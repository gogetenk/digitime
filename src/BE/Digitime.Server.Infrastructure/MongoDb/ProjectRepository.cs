using System.Linq.Expressions;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

internal class ProjectRepository : MongoRepository<ProjectEntity>, IProjectRepository
{
    public ProjectRepository(IMongoDbSettings settings) : base(settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        Collection = database.GetCollection<ProjectEntity>(settings.ProjectsCollectionName);
    }

    public Task DeleteManyAsync(Expression<Func<Project, bool>> filterExpression)
        => base.DeleteManyAsync(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>());

    public Task DeleteOneAsync(Expression<Func<Project, bool>> filterExpression)
        => base.DeleteOneAsync(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>());

    public IEnumerable<Project> FilterBy(Expression<Func<Project, bool>> filterExpression)
        => base.FilterBy(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>()).Adapt<IEnumerable<Project>>();

    public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<Project, bool>> filterExpression, Expression<Func<Project, TProjected>> projectionExpression)
        => base.FilterBy(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>()).Adapt<IEnumerable<TProjected>>();

    public Task<IEnumerable<Project>> FilterByAsync(Expression<Func<Project, bool>> filterExpression)
        => (base.FilterBy(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>())).Adapt<Task<IEnumerable<Project>>>();

    public async Task<Project> FindOneAsync(Expression<Func<Project, bool>> filterExpression)
        => (await base.FindOneAsync(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>())).Adapt<Project>();

    public Task InsertManyAsync(ICollection<Project> documents)
        => base.InsertManyAsync(documents.Adapt<ICollection<ProjectEntity>>());

    public Task InsertOneAsync(Project document)
        => base.InsertOneAsync(document.Adapt<ProjectEntity>());

    public Task ReplaceOneAsync(Project document)
        => base.ReplaceOneAsync(document.Adapt<ProjectEntity>());

    IQueryable<Project> IProjectRepository.AsQueryable()
        => base.AsQueryable().Adapt<IQueryable<Project>>();

    async Task<Project> IProjectRepository.FindByIdAsync(string id)
        => (await base.FindByIdAsync(id)).Adapt<Project>();
}
