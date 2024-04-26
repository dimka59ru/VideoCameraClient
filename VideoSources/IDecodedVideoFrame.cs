namespace App.VideoSources;

public interface IDecodedVideoFrame
{
    //void TransformTo(IntPtr buffer, int bufferStride, TransformParameters transformParameters);
    byte[] BgraPixelData { get; }
    int Width { get; }
    int Height { get; }
}