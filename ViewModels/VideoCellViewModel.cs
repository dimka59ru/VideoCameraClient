using System;
using App.VideoSources;

namespace App.ViewModels;

public class VideoCellViewModel : ViewModelBase, IDisposable
{
    public int Index { get; }
    public VideoPlayerViewModel VideoPlayerViewModel { get; }
    
    public VideoCellViewModel(int index, IVideoSource videoSource)
    {
        Index = index;
        VideoPlayerViewModel = new VideoPlayerViewModel(videoSource);
    }

    public void Dispose()
    {
        VideoPlayerViewModel.Dispose();
    }
}