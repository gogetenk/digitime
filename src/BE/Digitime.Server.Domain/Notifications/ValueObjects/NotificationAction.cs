namespace Digitime.Server.Domain.Notifications.ValueObjects;

public record NotificationAction
{
    public string Title { get; init; }
    public string Url { get; init; }
    public string BackgroundColor { get; init; }
    public string TextColor { get; init; }
}
