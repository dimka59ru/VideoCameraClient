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
        var vs = new RandomImagesVideoSource();
        VideoPlayerViewModel = new VideoPlayerViewModel(vs);
    }

    public void Dispose()
    {
        VideoPlayerViewModel.Dispose();
    }
}