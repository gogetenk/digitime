using System.Linq.Expressions;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Projects.ValueObjects;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using MongoDB.Driver;
using Project = Digitime.Server.Domain.Projects.Project;

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

    public async Task<List<Project>> GetProjectsByUserId(string userId)
    {
        var projects = await FilterByAsync(x => x.Members.Any(x => x.UserId == userId));
        return projects.Adapt<List<Project>>();
    }

    public async Task<Project> InsertOneAsync(Project project)
    {
        var projectEntity = project.Adapt<ProjectEntity>();
        await base.InsertOneAsync(projectEntity);
        var createdItem = await Collection.Find(x => x.Id == projectEntity.Id).FirstOrDefaultAsync();
        return createdItem.Adapt<Project>();
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        var projectEntity = project.Adapt<ProjectEntity>();
        await base.ReplaceOneAsync(projectEntity);
        var updatedItem = await Collection.Find(x => x.Id == projectEntity.Id).FirstOrDefaultAsync();
        return updatedItem.Adapt<Project>();
    }

    public async Task<ProjectMember> FindMemberByEmailAsync(string email)
    {
        var member = await Collection.Find(x => x.Members.Any(y => y.Email == email)).FirstOrDefaultAsync();
        return member.Adapt<ProjectMember>();
    }
}
