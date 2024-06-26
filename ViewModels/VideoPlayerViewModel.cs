using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using App.Models;
using App.VideoSources;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPlayerViewModel : ViewModelBase, IObserver<IDecodedVideoFrame>, IDisposable
{
    private readonly IVideoSource _videoSource;
    

    private bool _started;

    private bool Started 
    {
        get => _started;
        set => this.RaiseAndSetIfChanged(ref _started, value);
    }

    private WriteableBitmap? _frameImage;
    public WriteableBitmap? FrameImage
    {
        get =>_frameImage;
        private set => this.RaiseAndSetIfChanged(ref _frameImage, value);
    }
    
    public ICommand StartCommand { get; }
    private IObservable<bool> StartCommandCanExecute =>
        this.WhenAnyValue(x => x.Started, started => !started);
        
    public ICommand StopCommand { get; }
    private IObservable<bool> StopCommandCanExecute => 
        this.WhenAnyValue(x => x.Started);
    
    
    private readonly IDisposable _subscription;
    
    public VideoPlayerViewModel(IVideoSource videoSource, ChannelInfo channelInfo)
    {
        _videoSource = videoSource ?? throw new ArgumentNullException(nameof(videoSource));
        
        StartCommand = ReactiveCommand.Create(Start, StartCommandCanExecute);
        StopCommand = ReactiveCommand.Create(Stop, StopCommandCanExecute);
        
        // Подписываем данный плеер на получение кадров от видеоисточника
        _subscription =  videoSource.Subscribe(this);
        Start();
    }

    private void Start()
    {
        _videoSource.Start();
        Started = true;
    }
    
    private void Stop()
    {
        _videoSource.Stop();
        Started = false;
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
        Stop();
        if (_videoSource is IDisposable vs)
            vs.Dispose();
        _subscription.Dispose();
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(IDecodedVideoFrame frame)
    {
        FrameImage = CreateBitmapFromPixelData(frame.BgraPixelData, frame.Width, frame.Height);
    }
}