using Digitime.Server.Infrastructure.MongoDb;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("notifications")]
public class NotificationEntity : EntityBase
{
    public NotificationEntityTypeEnum Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public NotificationStyling? Style { get; set; }
    public NotificationAction? Action { get; set; }
    public DateTime Created { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public NotificationEntityStatusEnum Status { get; set; }
    public List<NotificationEntityChannelEnum> Channels { get; set; }
    public Dictionary<string,string> MetaData { get; set; }

    public enum NotificationEntityChannelEnum
    {
        InApp,
        Email,
        Push
    }

    public enum NotificationEntityTypeEnum
    {
        Info,
        Warning,
        Error
    }

    public enum NotificationEntityStatusEnum
    {
        Unread,
        Read,
        Sent,
        Failed
    }
}

public class NotificationAction
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string BackgroundColor { get; set; }
    public string TextColor { get; set; }
}

public class NotificationStyling
{
    public string Logo { get; set; }
    public string BackgroundColor { get; set; }
    public string TextColor { get; set; }
}
