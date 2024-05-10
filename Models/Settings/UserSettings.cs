using System.Collections.Generic;
using App.Infrastructure;

namespace App.Models.Settings;

public class UserSettings : SettingsManager<UserSettings>
{
    private Dictionary<int, VideoStreamSourceSettings>? _videoStreamSourceSettingsMap;
    public int LastOpenPanelIndex { get; set; }

    public Dictionary<int, VideoStreamSourceSettings> VideoStreamSourceSettingsMap
    {
        get => _videoStreamSourceSettingsMap ??= new Dictionary<int, VideoStreamSourceSettings>();
        set => _videoStreamSourceSettingsMap = value;
    }
}