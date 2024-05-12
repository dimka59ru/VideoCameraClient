using App.Models.Settings;

namespace App.Infrastructure.Settings;

public class UserSettingsManager : SettingsManager<UserSettings>
{
    public UserSettingsManager(ISettingsRepository<UserSettings> settingsRepository) : base(settingsRepository)
    {
    }
}