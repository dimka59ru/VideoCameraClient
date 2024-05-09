using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using App.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FFmpeg.AutoGen;
using ReactiveUI;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;
    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    private ListItemTemplate? _selectedListItem;
    public ListItemTemplate? SelectedListItem
    {
        get => _selectedListItem;
        set => this.RaiseAndSetIfChanged(ref _selectedListItem, value);
    }
    
    public ObservableCollection<ListItemTemplate> Items { get; } =
    [
        new ListItemTemplate(typeof(VideoPanelPageViewModel), "grid_regular"),
        new ListItemTemplate(typeof(SettingsPageViewModel), "settings_regular")
    ];

    public MainWindowViewModel()
    {
        SelectedListItem = Items[0]; // Set Current Page
        
        this.WhenAnyValue(x => x.SelectedListItem)
            .Subscribe(OnSelectedListItemChanged);
        
        FFmpegBinariesHelper.RegisterFFmpegBinaries();
        SetupFfmpegLogging();
        Console.WriteLine($"FFmpeg version info: {ffmpeg.av_version_info()}");
    }

    private void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        
        if (CurrentPage is IDisposable currentPage) 
            currentPage.Dispose();
        CurrentPage = (ViewModelBase)instance;
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

public class ListItemTemplate
{
    public string Label { get; set; }
    public Type ModelType { get; set; }
    public StreamGeometry ListItemIcon { get; }

    public ListItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "");
        
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }
}
