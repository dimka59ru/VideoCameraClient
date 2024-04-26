using System;
using System.Runtime.InteropServices;
using App.Services;
using App.VideoSources;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPlayerViewModel : ViewModelBase, IDisposable
{
    private readonly IVideoSource _videoSource;
    public IVideoPlayer VideoPlayer { get; }

    private WriteableBitmap? _frameImage;
    public WriteableBitmap? FrameImage
    {
        get =>_frameImage;
        private set => this.RaiseAndSetIfChanged(ref _frameImage, value);
    }
    
    public VideoPlayerViewModel(IVideoSource videoSource)
    {
        _videoSource = videoSource ?? throw new ArgumentNullException(nameof(videoSource));
        VideoPlayer = new VideoPlayer(videoSource);
        VideoPlayer.Start();
        _videoSource.FrameReceived += OnFrameReceived;
    }
    
    private void OnFrameReceived(object? sender, IDecodedVideoFrame e)
    {
        FrameImage = CreateBitmapFromPixelData(e.BgraPixelData, e.Width, e.Height);
    }

    private static WriteableBitmap CreateBitmapFromPixelData(byte[] bgraPixelData, int pixelWidth, int pixelHeight)
    {
        // Standard may need to change on some devices
        var dpi = new Vector(96, 96);

        var bitmap = new WriteableBitmap(
            new PixelSize(pixelWidth, pixelHeight),
            dpi,
            PixelFormat.Bgra8888,
            AlphaFormat.Premul);

        using var frameBuffer = bitmap.Lock();
        Marshal.Copy(bgraPixelData, 0, frameBuffer.Address, bgraPixelData.Length);

        return bitmap;
    }
    
    public void Dispose()
    {
        VideoPlayer.Dispose();
        _videoSource.FrameReceived -= OnFrameReceived;
    }
}