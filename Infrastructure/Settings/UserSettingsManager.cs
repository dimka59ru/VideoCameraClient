using App.Models.Settings;

namespace App.Infrastructure.Settings;

public class UserSettingsManager : JsonFileSettingsRepository<UserSettings>
{
}