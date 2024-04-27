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
        // 1 way
         // unsafe
         // {
         //     var ptr = (uint*)frameBuffer.Address;
         //     for(var x=0; x < frameBuffer.Size.Width; x++)
         //     for (var y = 0; y < frameBuffer.Size.Height; y++)
         //     {
         //         uint pixel = bgraPixelData[y * frameBuffer.RowBytes + x * 4 + 2]
         //                      | (uint)(bgraPixelData[y * frameBuffer.RowBytes + x * 4 + 1] << 8)
         //                      | (uint)(bgraPixelData[y * frameBuffer.RowBytes + x * 4 + 0] << 16)
         //                      | (uint)(bgraPixelData[y * frameBuffer.RowBytes + x * 4  + 3] << 24);
         //         ptr[y * frameBuffer.RowBytes / 4 + x] = pixel;
         //     }
         // }
        
        // 2 way
        Marshal.Copy(bgraPixelData, 0, frameBuffer.Address, bgraPixelData.Length);

        return bitmap;
    }
    
    public void Dispose()
    {
        VideoPlayer.Dispose();
        _videoSource.FrameReceived -= OnFrameReceived;
    }
}