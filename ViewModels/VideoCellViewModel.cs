using System;
using App.Models;
using App.VideoSources;

namespace App.ViewModels;

public class VideoCellViewModel : ViewModelBase, IDisposable
{
    public int Index { get; }
    public VideoPlayerViewModel VideoPlayerViewModel { get; }
    
    public VideoCellViewModel(int index)
    {
        Index = index;
        IVideoSource vs = new RandomImagesVideoSource();
        if (index == 1)
            //vs = new VideoStreamSource("rtsp://admin:Admin123@192.168.4.223:554/ch0/stream0");
            vs = new VideoStreamSource("http://158.58.130.148/mjpg/video.mjpg");
        if (index == 2)
            vs = new VideoStreamSource("http://158.58.130.148/mjpg/video.mjpg");
        if (index == 3)
            vs = new VideoStreamSource("http://61.211.241.239/nphMotionJpeg?Resolution=1280x720&Quality=Standard");
        if (index == 4)
            vs = new VideoStreamSource("http://tamperehacklab.tunk.org:38001/nphMotionJpeg?Resolution=1280x720&Quality=Clarity");
        if (index == 5)
            vs = new VideoStreamSource("http://takemotopiano.aa1.netvolante.jp:8190/nphMotionJpeg?Resolution=640x480&Quality=Standard&Framerate=30");
        if (index == 6)
            vs = new VideoStreamSource("http://honjin1.miemasu.net/nphMotionJpeg?Resolution=640x480&Quality=Standard");
        if (index == 7)
            //vs = new RTSPvideoSource("http://47.51.131.147/-wvhttp-01-/GetOneShot?image_size=1280x720&frame_count=1000000000");
            //vs = new RTSPvideoSource("http://pendelcam.kip.uni-heidelberg.de/mjpg/video.mjpg");
            vs = new VideoStreamSource("http://camera.buffalotrace.com/mjpg/video.mjpg");
        if (index == 8)
            vs = new VideoStreamSource("http://webcam.mchcares.com/mjpg/video.mjpg?timestamp=1566232173730");
        if (index == 9)
            vs = new VideoStreamSource("http://77.222.181.11:8080/mjpg/video.mjpg");
        
        
        VideoPlayerViewModel = new VideoPlayerViewModel(vs, new ChannelInfo(index, index.ToString()));
    }

    public void Dispose()
    {
        VideoPlayerViewModel.Dispose();
    }
}