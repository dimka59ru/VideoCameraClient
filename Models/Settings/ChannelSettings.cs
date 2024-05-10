using System;

namespace App.Models.Settings;

public record ChannelSettings(string? Name, Uri? MainStreamUri, Uri? SubStreamUri, TimeSpan? PingPeriod);