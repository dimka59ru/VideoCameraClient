using System;
using System.Threading.Tasks;

namespace App.VideoSources;
public class RandomImagesVideoSource : IVideoSource
{
    public event EventHandler<IDecodedVideoFrame>? FrameReceived;
    
    private const int ImageWidth = 1920;
    private const int ImageHeight = 1080;
    
    private readonly byte[] _bgraPixelData = new byte[ImageWidth * ImageHeight * 4];
    public RandomImagesVideoSource()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var rand = new Random();

                for (var i = 0; i < _bgraPixelData.Length; i+=4)
                {
                    _bgraPixelData[i] = (byte)rand.Next(0, 255);   //B
                    _bgraPixelData[i+1] = (byte)rand.Next(0, 255); //G
                    _bgraPixelData[i+2] = (byte)rand.Next(0, 255); //R
                    _bgraPixelData[i+3] = 255;                     //A
                }
                
                var frame = new DecodedVideoFrame(_bgraPixelData, ImageWidth, ImageHeight);
                
                FrameReceived?.Invoke(this, frame);

                await Task.Delay(50);
            }
        });
    }
}