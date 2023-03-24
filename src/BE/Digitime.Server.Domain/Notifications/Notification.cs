using System;
using System.Collections.Generic;
using Digitime.Server.Domain.Notifications.ValueObjects;

namespace Digitime.Server.Domain.Notifications;

public class Notification
{
    public string Id { get; set; }
    public NotificationTypeEnum Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public NotificationStyling? Style { get; set; }
    public NotificationAction? Action { get; set; }
    public DateTime Created { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public NotificationStatusEnum Status { get; set; }
    public List<NotificationChannelEnum> Channels { get; set; }
    public Dictionary<string, string> MetaData { get; set; } = new();

    public Notification() { }

    // create method to initialize
    public static Notification Create(
        NotificationTypeEnum type, 
        string title, 
        string message, 
        string userId, 
        NotificationStyling? style, 
        NotificationAction? action, 
        DateTime created, 
        DateTime? scheduledDate, 
        NotificationStatusEnum status, 
        List<NotificationChannelEnum> channels, 
        Dictionary<string, string> metaData = null)
    {
        return new Notification
        {
            Type = type,
            Title = title,
            Message = message,
            UserId = userId,
            Style = style,
            Action = action,
            Created = created,
            ScheduledDate = scheduledDate,
            Status = status,
            Channels = channels,
            MetaData = metaData
        };
    }

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

