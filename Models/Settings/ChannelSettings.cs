using System;

namespace App.Models.Settings;

public class ChannelSettings
{
    public string? Name { get; set; }
    public Uri? MainStreamUri { get; set; }
    public Uri? SubStreamUri { get; set; }
    public TimeSpan? PingPeriod { get; set; }
}