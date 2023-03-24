using System.Collections.Generic;

namespace Digitime.Server.Domain.Projects.ValueObjects;

public record ProjectSettings
{
    public Dictionary<string, NotificationSetting> NotificationSettings { get; private set; }

    public ProjectSettings(Dictionary<string, NotificationSetting> notificationSettings)
    {
        NotificationSettings = notificationSettings;
    }

    public void UpdateNotificationSetting(string key, NotificationSetting newSetting)
    {
        if (NotificationSettings.ContainsKey(key))
        {
            NotificationSettings[key] = newSetting;
        }
        else
        {
            NotificationSettings.Add(key, newSetting);
        }
    }
}
