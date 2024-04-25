using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace App.ViewModels;

public class MainVideoPanelViewModel : ViewModelBase
{
    private ViewModelBase? _currentPanel;
    
    public ViewModelBase? CurrentPanel
    {
        get => _currentPanel;
        set => this.RaiseAndSetIfChanged(ref _currentPanel, value);
    }

    public ReactiveCommand<string, Unit> OpenVideoPanelCommand { get; }

    public MainVideoPanelViewModel()
    {
        OpenVideoPanelCommand = ReactiveCommand.Create<string>(OpenVideoPanel);
        CurrentPanel = new FourCellsPanelViewModel();
    }

    private void OpenVideoPanel(string countCells)
    {
        switch (countCells)
        {
            case "four":
                CurrentPanel = new FourCellsPanelViewModel();
                break;
            case "nine":
                CurrentPanel = new NineCellsPanelViewModel();
                break;
            default:
                CurrentPanel = new FourCellsPanelViewModel();
                break;
        }
    }

    //private void VideoPanel() => CurrentPanel = new VideoPanelViewModel();
}