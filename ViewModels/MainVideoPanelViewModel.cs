using System.Reactive;
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
        CurrentPanel = countCells switch
        {
            "four" => new FourCellsPanelViewModel(),
            "nine" => new NineCellsPanelViewModel(),
            _ => new FourCellsPanelViewModel()
        };
    }
}