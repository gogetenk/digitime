namespace Digitime.Shared.Dto;

public class NotificationDto
{
    public string Id { get; set; }
    public NotificationTypeEnum Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public NotificationStylingDto? Style { get; set; }
    public NotificationActionDto? Action { get; set; }
    public DateTime Created { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public NotificationStatusEnum Status { get; set; }
    public List<NotificationChannelEnum> Channels { get; set; }
    public Dictionary<string, string> MetaData { get; set; } = new();

    public enum NotificationChannelEnum
    {
        InApp,
        Email,
        Push
    }

    public enum NotificationTypeEnum
    {
        Info,
        Warning,
        Error
    }

    public enum NotificationStatusEnum
    {
        Unread,
        Read,
        Sent,
        Failed
    }
}

public record NotificationStylingDto
{
    public string Logo { get; init; }
    public string BackgroundColor { get; init; }
    public string TextColor { get; init; }
}

public record NotificationActionDto
{
    public string Title { get; init; }
    public string Url { get; init; }
    public string BackgroundColor { get; init; }
    public string TextColor { get; init; }
}
