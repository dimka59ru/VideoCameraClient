using System;

namespace App.VideoSources;

public interface IVideoSource
{
    event EventHandler<IDecodedVideoFrame> FrameReceived;

    void Start();
    void Stop();
}