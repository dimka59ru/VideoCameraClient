using System;

namespace App.Models.Settings;

public class VideoStreamSourceSettings
{
    public Uri? MainStreamUri { get; set; }
    public Uri? SubStreamUri { get; set; }
    public TimeSpan PingPeriod { get; set; }
}