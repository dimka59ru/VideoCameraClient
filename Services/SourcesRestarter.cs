using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.VideoSources;

namespace App.Services;

/// <summary>
/// Для перезапуска источников видео
/// Крайне странный класс, переписать
/// </summary>
public class SourcesRestarter
{
    private readonly List<IVideoSource> _sources = [];
    private static SourcesRestarter _instance;
    
    public static SourcesRestarter Instance => LazyInitializer.EnsureInitialized(ref _instance, () => new SourcesRestarter()); 
        
    private SourcesRestarter()
    {
        Task.Run(CheckAndRestart);
    }
    
    public void Add(IVideoSource videoSource)
    {
        _sources.Add(videoSource);
    }
    
    public void Remove(IVideoSource videoSource)
    {
        _sources.Remove(videoSource);
    }

    private void CheckAndRestart()
    {
        while (true)
        {
            try
            {
                var stoppedSources = _sources.Where(x => !x.IsAlive).ToList();

                // Если не живые источники есть, то
                // ждем 20 секунд (может стартанули) и проверяем снова
                if (stoppedSources.Count > 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                    stoppedSources = _sources.Where(x => !x.IsAlive).ToList();
                    
                    // если все еще есть, то останвливаем их
                    foreach (var source in stoppedSources)
                    {
                        source.Stop();
                        Console.WriteLine($"source stopped.");
                    }
                    
                    // даем 5 секунд, чтобы завершились
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    
                    // Запускаем снова
                    foreach (var source in stoppedSources)
                    {
                        source.Start();
                        Console.WriteLine($"source started.");
                    }
                    
                    // даем 15 секунд на запуск
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}