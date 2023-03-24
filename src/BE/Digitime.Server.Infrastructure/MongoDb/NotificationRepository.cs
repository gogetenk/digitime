using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.MongoDb;

public class NotificationRepository : MongoRepository<NotificationEntity>, INotificationRepository
{
    public NotificationRepository(IMongoDbSettings settings) : base(settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        Collection = database.GetCollection<NotificationEntity>(settings.NotificationsCollectionName);
    }

    public Task CreateAsync(Notification notification)
    {
        return base.InsertOneAsync(notification.Adapt<NotificationEntity>());
    }

    public Task DeleteAsync(string id)
    {
        return DeleteByIdAsync(id);
    }

    public async Task<List<Notification>> GetNotificationsAsync(string userId)
    {
        try
        {
            var projects = await FilterByAsync(x => x.UserId == userId);
            return projects.Adapt<List<Notification>>();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
