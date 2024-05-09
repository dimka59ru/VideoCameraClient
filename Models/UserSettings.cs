using App.Infrastructure;

namespace App.Models;

public class UserSettings : SettingsManager<UserSettings>
{
    public int LastOpenPanelIndex { get; set; }
}