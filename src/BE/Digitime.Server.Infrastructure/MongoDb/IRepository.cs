using System.Linq.Expressions;
using Digitime.Server.Infrastructure.Entities;

namespace Digitime.Server.Infrastructure.MongoDb;
public interface IRepository<TDocument> where TDocument : EntityBase
{
    IQueryable<TDocument> AsQueryable();

    IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);


    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task<TDocument> FindByIdAsync(string id);

    Task InsertOneAsync(TDocument document);

    Task InsertManyAsync(ICollection<TDocument> documents);

    Task ReplaceOneAsync(TDocument document);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteByIdAsync(string id);

    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}
