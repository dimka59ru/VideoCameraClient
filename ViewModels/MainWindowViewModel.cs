using System;
using System.Collections.ObjectModel;
using App.Infrastructure.Settings;
using App.Models.Settings;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ISettingsRepository<UserSettings> _userSettingsManager;
    private ViewModelBase? _currentPage;
    public ViewModelBase? CurrentPage
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
        new ListItemTemplate(typeof(VideoPanelPageViewModel), "GridRegular"),
        new ListItemTemplate(typeof(SettingsPageViewModel), "SettingsRegular")
    ];

    public MainWindowViewModel(ISettingsRepository<UserSettings> userSettingsManager)
    {
        _userSettingsManager = userSettingsManager ?? throw new ArgumentNullException(nameof(userSettingsManager));
        
        this.WhenAnyValue(x => x.SelectedListItem)
            .Subscribe(OnSelectedListItemChanged);
        
        SelectedListItem = Items[0]; // Set Current Page
    }

    private void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        if (CurrentPage is IDisposable currentPage) 
            currentPage.Dispose();

        var type = value.ModelType;
        
        CurrentPage = true switch
        {
            true when type == typeof(VideoPanelPageViewModel) => new VideoPanelPageViewModel(_userSettingsManager),
            true when type == typeof(SettingsPageViewModel) => new SettingsPageViewModel(),
            _ => CurrentPage
        };
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
