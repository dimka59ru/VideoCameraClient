using System.Collections.Generic;
using App.Infrastructure;

namespace App.Models.Settings;

public class UserSettings : SettingsManager<UserSettings>
{
    private Dictionary<int, ChannelSettings>? _channelSettingsMap;
    public int LastOpenPanelIndex { get; set; }

    public Dictionary<int, ChannelSettings> ChannelSettingsMap
    {
        get => _channelSettingsMap ??= new Dictionary<int, ChannelSettings>();
        set => _channelSettingsMap = value;
    }
}