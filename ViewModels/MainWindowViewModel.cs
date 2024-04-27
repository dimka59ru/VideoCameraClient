using System;
using System.Windows.Input;
using ReactiveUI;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get =>_currentView;
        set
        {
            if (_currentView is IDisposable view)
                view.Dispose();
            this.RaiseAndSetIfChanged(ref _currentView, value);
            RaiseProperties();
        }
    }

    public ICommand OpenVideoPanelCommand { get; }
    public ICommand MainSettingsCommand { get; }
    
    public bool IsVideoPanelSelected => CurrentView is MainVideoPanelViewModel;
    public bool IsMainSettingSelected => CurrentView is MainSettingsViewModel;

    public MainWindowViewModel()
    {
        OpenVideoPanelCommand = ReactiveCommand.Create(VideoPanel);
        MainSettingsCommand = ReactiveCommand.Create(MainSettings);
        
        CurrentView = new MainVideoPanelViewModel();
    }
    
    private void VideoPanel()
    {
        CurrentView = new MainVideoPanelViewModel();
    }

    private void MainSettings()
    {
        CurrentView = new MainSettingsViewModel();
    }

    private void RaiseProperties()
    {
        this.RaisePropertyChanged(nameof(IsVideoPanelSelected));
        this.RaisePropertyChanged(nameof(IsMainSettingSelected));
    }
}
