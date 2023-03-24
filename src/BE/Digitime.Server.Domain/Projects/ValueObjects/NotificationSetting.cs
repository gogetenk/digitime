using System;
using System.Collections.Generic;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Domain.Projects.ValueObjects;

public record NotificationSetting
{
    public DateTime? ReminderDate { get; private set; }
    public PeriodicityEnum Periodicity { get; private set; }
    public List<Notification.NotificationChannelEnum> ReminderChannels { get; private set; }

    public NotificationSetting(DateTime? reminderDate, PeriodicityEnum periodicity, List<Notification.NotificationChannelEnum> reminderChannels)
    {
        ReminderDate = reminderDate;
        Periodicity = periodicity;
        ReminderChannels = reminderChannels;
    }
}

public enum PeriodicityEnum
{
    None,
    Daily,
    Weekly,
    Monthly,
    Quarterly,
    Yearly
}