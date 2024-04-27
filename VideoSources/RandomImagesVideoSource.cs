using System;
using System.Reactive.Linq;
using Avalonia.Media;

namespace App.VideoSources;
public class RandomImagesVideoSource : IVideoSource
{
    public event EventHandler<IDecodedVideoFrame>? FrameReceived;

    private const int ImageWidth = 1920;
    private const int ImageHeight = 1080;
    
    private readonly byte[] _bgraPixelData = new byte[ImageWidth * ImageHeight * 4];
    private IDisposable? _subscription;
    
    public void Start()
    {
        Stop();
        _subscription =
            Observable
                .Interval(TimeSpan.FromMilliseconds(40))
                .Subscribe(x =>
                {
                    var frame = GetFrame();
                    FrameReceived?.Invoke(this, frame);
                });
        
        Console.WriteLine("Source Started");
        
       
    }

    public void Stop()
    {
        _subscription?.Dispose();
        Console.WriteLine("Source Stopped");
    }

    private DecodedVideoFrame GetFrame()
    {
        var rand = new Random();
        Color c = Color.FromArgb(
            (byte)rand.Next(0, 255),
            (byte)rand.Next(0, 255), 
            (byte)rand.Next(0, 255), 
            (byte)rand.Next(0, 255));
        
        for (var i = 0; i < _bgraPixelData.Length; i+=4)
        {
            _bgraPixelData[i] = c.B;     //B
            _bgraPixelData[i + 1] = c.G; //G
            _bgraPixelData[i + 2] = c.R; //R
            _bgraPixelData[i + 3] = c.A; //A
        }
        //rand.NextBytes(_bgraPixelData);
        return  new DecodedVideoFrame(_bgraPixelData, ImageWidth, ImageHeight);
    }
}