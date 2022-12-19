using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Digitime.Server.Domain.Projects;

namespace Digitime.Server.Application.Abstractions;

public interface IProjectRepository
{
    IQueryable<Project> AsQueryable();

    IEnumerable<Project> FilterBy(Expression<Func<Project, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<Project, bool>> filterExpression, Expression<Func<Project, TProjected>> projectionExpression);

    Task<Project> FindOneAsync(Expression<Func<Project, bool>> filterExpression);

    Task<Project> FindByIdAsync(string id);

    Task<IEnumerable<Project>> FilterByAsync(Expression<Func<Project, bool>> filterExpression);

    Task InsertOneAsync(Project document);

    Task InsertManyAsync(ICollection<Project> documents);

    Task ReplaceOneAsync(Project document);

    Task DeleteOneAsync(Expression<Func<Project, bool>> filterExpression);

    Task DeleteByIdAsync(string id);

    Task DeleteManyAsync(Expression<Func<Project, bool>> filterExpression);
}
