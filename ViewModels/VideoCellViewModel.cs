using System;
using App.Infrastructure.Settings;
using App.Models;
using App.Models.Settings;
using App.VideoSources;
using ReactiveUI;

namespace App.ViewModels;

public class VideoCellViewModel : ViewModelBase, IDisposable
{
    public int Index { get; }
    public VideoPlayerViewModel? VideoPlayerViewModel { get; }
    
    private string? _channelName;
    public string? ChannelName
    {
        get => _channelName;
        set => this.RaiseAndSetIfChanged(ref _channelName, value);
    }
    
    public VideoCellViewModel(int index, SettingsManager<UserSettings> userSettingsManager)
    {
        ArgumentNullException.ThrowIfNull(userSettingsManager);

        var userSettings = userSettingsManager.Load();
        var channelSettingsMap = userSettings.ChannelSettingsMap;
        Index = index;
        
        //IVideoSource vs = new RandomImagesVideoSource();
        // switch (index)
        // {
        //     case 1:
        //     {
        //         //vs = new VideoStreamSource("rtsp://admin:Admin123@192.168.4.223:554/ch0/stream0");
        //         var uri = videoStreamSourceSettingsMap[1].MainStreamUri;
        //         vs = new VideoStreamSource(uri.ToString());
        //         break;
        //     }
        //     case 2:
        //         vs = new VideoStreamSource("http://158.58.130.148/mjpg/video.mjpg");
        //         break;
        //     case 3:
        //         vs = new VideoStreamSource("http://61.211.241.239/nphMotionJpeg?Resolution=1280x720&Quality=Standard");
        //         break;
        //     case 4:
        //         vs = new VideoStreamSource("http://tamperehacklab.tunk.org:38001/nphMotionJpeg?Resolution=1280x720&Quality=Clarity");
        //         break;
        //     case 5:
        //         vs = new VideoStreamSource("http://takemotopiano.aa1.netvolante.jp:8190/nphMotionJpeg?Resolution=640x480&Quality=Standard&Framerate=30");
        //         break;
        //     case 6:
        //         vs = new VideoStreamSource("http://honjin1.miemasu.net/nphMotionJpeg?Resolution=640x480&Quality=Standard");
        //         break;
        //     //vs = new RTSPvideoSource("http://47.51.131.147/-wvhttp-01-/GetOneShot?image_size=1280x720&frame_count=1000000000");
        //     //vs = new RTSPvideoSource("http://pendelcam.kip.uni-heidelberg.de/mjpg/video.mjpg");
        //     case 7:
        //         vs = new VideoStreamSource("http://camera.buffalotrace.com/mjpg/video.mjpg");
        //         break;
        //     case 8:
        //         vs = new VideoStreamSource("http://webcam.mchcares.com/mjpg/video.mjpg?timestamp=1566232173730");
        //         break;
        //     case 9:
        //         vs = new VideoStreamSource("http://77.222.181.11:8080/mjpg/video.mjpg");
        //         break;
        // }
       
        if (channelSettingsMap.TryGetValue(index, out var channelSettings))
        {
            var uri = channelSettings.MainStreamUri;
            if (uri != null)
            {
                var vs = new VideoStreamSource(uri.ToString());
                VideoPlayerViewModel = new VideoPlayerViewModel(vs, new ChannelInfo(index, index.ToString()));
            }

            ChannelName = channelSettings.Name;
        }
    }

    public void Dispose()
    {
        VideoPlayerViewModel?.Dispose();
    }
}