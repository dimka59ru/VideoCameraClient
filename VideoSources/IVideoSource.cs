using System;

namespace App.VideoSources;

public interface IVideoSource : IObservable<IDecodedVideoFrame>
{
    void Start();
    void Stop();

    bool IsAlive { get; }
}