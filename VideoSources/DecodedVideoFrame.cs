namespace App.VideoSources;

public class DecodedVideoFrame : IDecodedVideoFrame
{
    public byte[] BgraPixelData { get; }
    public int Width { get; }
    public int Height { get; }

    public DecodedVideoFrame(byte[] bgraPixelData, int width, int height)
    {
        BgraPixelData = bgraPixelData;
        Width = width;
        Height = height;
    }
}