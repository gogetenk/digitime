using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : EntityBase
{
    protected IMongoCollection<TDocument> Collection { get; set; }

    public MongoRepository(IMongoDbSettings settings)
    {
    }

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return Collection.AsQueryable();
    }

    public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Collection.Find(filterExpression).ToEnumerable();
    }

    public virtual async Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return (await Collection.FindAsync(filterExpression)).ToEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return Collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await Collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public async virtual Task<TDocument> FindByIdAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        return await Collection.Find(x => x.Id == id).SingleOrDefaultAsync();
    }

    public virtual async Task InsertOneAsync(TDocument document)
    {
        await Collection.InsertOneAsync(document);
    }

    public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        await Collection.InsertManyAsync(documents);
    }

    public virtual async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await Collection.FindOneAndReplaceAsync(filter, document);
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await Collection.FindOneAndDeleteAsync(filterExpression);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var objectId = new ObjectId(id);
        await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        Collection.DeleteMany(filterExpression);
    }

    public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await Collection.DeleteManyAsync(filterExpression);
    }
}