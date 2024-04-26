using System;
using App.VideoSources;

namespace App.Services;

public interface IVideoPlayer : IDisposable
{
    IVideoSource VideoSource { get; }
    
    void Start();
    void Stop();
}