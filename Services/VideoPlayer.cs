using System;
using App.VideoSources;

namespace App.Services;

public sealed class VideoPlayer : IVideoPlayer
{
    public IVideoSource VideoSource { get; }

    public VideoPlayer(IVideoSource videoSource)
    {
        VideoSource = videoSource ?? throw new ArgumentNullException(nameof(videoSource));
    }
    
    public void Start()
    {
        Console.WriteLine("Player Started");
    }

    public void Stop()
    {
        Console.WriteLine("Player Stopped");
    }

    public void Dispose()
    {
        Stop();
    }
}