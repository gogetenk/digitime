using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitime.Server.Infrastructure.Entities;
public abstract class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId Id { get; set; }
}
