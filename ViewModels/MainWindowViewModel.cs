using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
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
