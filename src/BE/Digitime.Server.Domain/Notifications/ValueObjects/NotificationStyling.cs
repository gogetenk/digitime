namespace Digitime.Server.Domain.Notifications.ValueObjects;
public record NotificationStyling
{
    public string Logo { get; init; }
    public string BackgroundColor { get; init; }
    public string TextColor { get; init; }
}
