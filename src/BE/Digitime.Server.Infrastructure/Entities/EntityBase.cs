using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitime.Server.Infrastructure.Entities;
public abstract class EntityBase
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
