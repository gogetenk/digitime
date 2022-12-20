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

    public async Task<Project> FindOneAsync(Expression<Func<Project, bool>> filterExpression)
        => (await base.FindOneAsync(filterExpression.Adapt<Expression<Func<ProjectEntity, bool>>>())).Adapt<Project>();

    public async Task<List<Project>> GetProjectsByReviewerId(string reviewerId)
    {
        var projects = await FilterByAsync(x => x.Members.Any(x => x.UserId == reviewerId && x.MemberRole == MemberRoleEntityEnum.Reviewer));
        return projects.Adapt<List<Project>>();
    }
    public async Task<Project> FindByIdAsync(string id)
        => (await base.FindByIdAsync(id)).Adapt<Project>();
}
