using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace App.ViewModels;

public class MainVideoPanelViewModel : ViewModelBase
{
    private int _columnCount;
    private int _rowCount;
    
    public int ColumnCount
    {
        get => _columnCount;
        set => this.RaiseAndSetIfChanged(ref _columnCount, value);
    }
    
    public int RowCount
    {
        get => _rowCount;
        set => this.RaiseAndSetIfChanged(ref _rowCount, value);
    }
    
    public ObservableCollection<int> Items { get; } = [];
    public ReactiveCommand<string, Unit> OpenVideoPanelCommand { get; }

    public MainVideoPanelViewModel()
    {
        OpenVideoPanelCommand = ReactiveCommand.Create<string>(OpenVideoPanel);
        UpdateVideoPanel(4);
    }

    private void OpenVideoPanel(string countCells)
    {
        switch (countCells)
        {
            case "four":
                UpdateVideoPanel(4);
                break;
            case "nine":
                UpdateVideoPanel(9);
                break;
            case "sixteen":
                UpdateVideoPanel(16);
                break;
            case "twenty_five":
                UpdateVideoPanel(25);
                break;
            default:
                UpdateVideoPanel(4);
                break;
        }
    }

    private void UpdateVideoPanel(int countCells)
    {
        RowCount = (int)Math.Sqrt(countCells);
        ColumnCount = (int)Math.Sqrt(countCells);

        var requiredCells = RowCount * ColumnCount;
        var currentCountCells = Items.Count;
        var addCell = requiredCells - currentCountCells;
        
        for (var i = currentCountCells; i <= requiredCells; i++)
        {
            Items.Add(i);
        }
        for (var i = currentCountCells; i >= requiredCells; i--)
        {
            Items.Remove(i);
        }
    }
}