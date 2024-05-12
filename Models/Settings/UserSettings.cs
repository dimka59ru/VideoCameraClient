using System.Collections.Generic;

namespace App.Models.Settings;

public class UserSettings
{
    public int LastOpenPanelIndex { get; set; }

    private Dictionary<int, ChannelSettings>? _channelSettingsMap;
    public Dictionary<int, ChannelSettings> ChannelSettingsMap
    {
        get => _channelSettingsMap ??= new Dictionary<int, ChannelSettings>();
        set => _channelSettingsMap = value;
    }
}