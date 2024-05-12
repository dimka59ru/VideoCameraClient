using System;
using System.Runtime.InteropServices;
using App.Infrastructure.Settings;
using App.Models.Settings;
using App.Services;
using App.ViewModels;
using App.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FFmpeg.AutoGen;

namespace App;

public partial class App : Application
{
    public override void Initialize()
    {
        FFmpegBinariesHelper.RegisterFFmpegBinaries();
        SetupFfmpegLogging();
        Console.WriteLine($"FFmpeg version info: {ffmpeg.av_version_info()}");
        
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        //Initialize dependencies
        var settingsRepository = new JsonFileSettingsRepository<UserSettings>();
        var userSettingsManager = new UserSettingsManager(settingsRepository);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(userSettingsManager),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private static unsafe void SetupFfmpegLogging()
    {
        ffmpeg.av_log_set_level(ffmpeg.AV_LOG_VERBOSE);

        // do not convert to local function
        av_log_set_callback_callback logCallback = (p0, level, format, vl) =>
        {
            if (level > ffmpeg.av_log_get_level()) return;

            var lineSize = 1024;
            var lineBuffer = stackalloc byte[lineSize];
            var printPrefix = 1;
            ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
            var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(line);
            Console.ResetColor();
        };

        ffmpeg.av_log_set_callback(logCallback);
    }
}