using System;
using System.Threading;
using System.Threading.Tasks;
using App.VideoSources;

namespace App.Services;

public class VideoSourceWatchdog : IObserver<IDecodedVideoFrame>, IDisposable
{
    private readonly TimeSpan _checkPeriod;
    private readonly Action _startProc;
    private readonly Action _stopProc;
    private DateTime? _lastFrameReceivedTime;
    private readonly Timer _stateTimer;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public VideoSourceWatchdog(TimeSpan checkPeriod, Action startProc, Action stopProc)
    {
        _checkPeriod = checkPeriod;
        _startProc = startProc ?? throw new ArgumentNullException(nameof(startProc));
        _stopProc = stopProc ?? throw new ArgumentNullException(nameof(stopProc));
        _stateTimer = new Timer(OnTimer, null, (int)checkPeriod.TotalMilliseconds, Timeout.Infinite);
    }

    private int startupAttempts = 3;
    private int restartAttempts = 5;
    
    private async void OnTimer(object? state)
    {
        try
        {
            // wait for start
            if (_lastFrameReceivedTime == null)
            {
                if (--startupAttempts > 0)
                    return;

                await RestartSource();
            }

            var now = DateTime.UtcNow;
            if (now - _lastFrameReceivedTime > _checkPeriod)
            {
                //not Alive
                await RestartSource();
            }
        }
        finally
        {
            _stateTimer.Change((int)_checkPeriod.TotalMilliseconds, Timeout.Infinite);
        }
    }

    private async Task RestartSource()
    {
        _stopProc();
        // даем 5 секунд, чтобы завершились
        await Task.Delay(TimeSpan.FromSeconds(5));
        _startProc();

        await _semaphoreSlim.WaitAsync();
        try
        {
            _lastFrameReceivedTime = null;
        }
        catch (Exception e)
        {
            
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }


    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(IDecodedVideoFrame value)
    {
        _semaphoreSlim.Wait();
        try
        {
            _lastFrameReceivedTime = DateTime.UtcNow;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public void Dispose()
    {
        _stateTimer.Dispose();
    }
}