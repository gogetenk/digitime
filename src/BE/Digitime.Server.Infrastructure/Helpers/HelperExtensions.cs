using MongoDB.Bson;

namespace Digitime.Server.Infrastructure.Helpers;

public static class HelperExtensions
{
    public static ObjectId ToObjectId(this string id)
    {
        return ObjectId.Parse(id);
    }
}
