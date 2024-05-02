using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using FFmpeg.AutoGen;

namespace App.VideoSources;

public class VideoStreamSource : IVideoSource, IDisposable
{
    private readonly string _url;
    private readonly List<IObserver<IDecodedVideoFrame>> _frameObservers = [];
    private CancellationTokenSource? _cancellationTokenSource;
    private DateTime _lastFrameReceivedTime = DateTime.MinValue;

    public bool IsAlive => DateTime.UtcNow - _lastFrameReceivedTime < TimeSpan.FromSeconds(5);

    public VideoStreamSource(string url)
    {
        _url = url;
        FFmpegBinariesHelper.RegisterFFmpegBinaries();
        Console.WriteLine($"FFmpeg version info: {ffmpeg.av_version_info()}");
    }

    public IDisposable Subscribe(IObserver<IDecodedVideoFrame> observer)
    {
        _frameObservers.Add(observer);
        return new Unsubscriber<IDecodedVideoFrame>(_frameObservers, observer);
    }

    public void Start()
    {
        SourcesRestarter.Instance.Add(this);
        
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => DecodeVideoStream(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    public void Stop()
    {
        SourcesRestarter.Instance.Remove(this);
        _cancellationTokenSource?.Cancel();
    }
    
    private void DecodeVideoStream(CancellationToken token)
    {
        using var vsd = new VideoStreamDecoder(_url);
        Console.WriteLine($"codec name: {vsd.CodecName}");
        var info = vsd.GetContextInfo();
        info.ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));
            
        var sourceSize = vsd.FrameSize;
        var sourcePixelFormat = vsd.PixelFormat;
            
        var destinationSize = sourceSize;
        var destinationPixelFormat = AVPixelFormat.@AV_PIX_FMT_BGRA;
            
        using var vfc = new VideoFrameConverter(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat);
            
        while (vsd.TryDecodeNextFrame(out var frame))
        {
            try
            {
                if(token.IsCancellationRequested)
                {
                    break;
                }
            }
            catch (ObjectDisposedException)
            {
                break;
            }

            _lastFrameReceivedTime = DateTime.UtcNow;
            
            unsafe
            {
                var convertedFrame = vfc.Convert(frame);
                var len = convertedFrame.width * convertedFrame.height * (convertedFrame.linesize[0] / convertedFrame.width);
                var arr = new byte[len];
                Marshal.Copy((IntPtr)convertedFrame.data[0], arr, 0, len);
                    
                var df = new DecodedVideoFrame(arr, convertedFrame.width, convertedFrame.height);
            
                SendFrameToObservers(df);
            }
        }
    }

    private void SendFrameToObservers(IDecodedVideoFrame df)
    {
        foreach (var observer in _frameObservers.ToList()) // https://stackoverflow.com/a/604843/6128692
        {
            observer.OnNext(df);
        }
    }
    

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
    }
}