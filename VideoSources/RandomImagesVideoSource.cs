using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using App.Models;

namespace App.VideoSources;
public class RandomImagesVideoSource : IVideoSource
{
    private const int ImageWidth = 850;
    private const int ImageHeight = 480;
    
    private readonly byte[] _bgraPixelData = new byte[ImageWidth * ImageHeight * 4];
    private IDisposable? _subscription;
    private readonly List<IObserver<IDecodedVideoFrame>> _frameObservers = [];

    public void Start()
    {
        Stop();
        _subscription =
            Observable
                .Interval(TimeSpan.FromMilliseconds(40))
                .Subscribe(x =>
                {
                    var frame = GetFrame();
                     foreach (var observer in _frameObservers.ToList()) // https://stackoverflow.com/a/604843/6128692
                     {
                         observer.OnNext(frame);
                     }
                });
        
        Console.WriteLine("Source Started");
    }

    public void Stop()
    {
        _subscription?.Dispose();
        Console.WriteLine("Source Stopped");
    }

    public bool IsAlive => true;

    private DecodedVideoFrame GetFrame()
    {
        var rand = new Random();
        // var c = Color.FromArgb(
        //     (byte)rand.Next(0, 255),
        //     (byte)rand.Next(0, 255), 
        //     (byte)rand.Next(0, 255), 
        //     (byte)rand.Next(0, 255));
        //
        // for (var i = 0; i < _bgraPixelData.Length; i+=4)
        // {
        //     _bgraPixelData[i] = c.B;     //B
        //     _bgraPixelData[i + 1] = c.G; //G
        //     _bgraPixelData[i + 2] = c.R; //R
        //     _bgraPixelData[i + 3] = c.A; //A
        // }
        rand.NextBytes(_bgraPixelData);
        return  new DecodedVideoFrame(_bgraPixelData, ImageWidth, ImageHeight);
    }

    public IDisposable Subscribe(IObserver<IDecodedVideoFrame> observer)
    {
        _frameObservers.Add(observer);
        return new Unsubscriber<IDecodedVideoFrame>(_frameObservers, observer);
    }
}