using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace App.ViewModels;

public class MainVideoPanelViewModel : ViewModelBase, IDisposable
{
    private int _columnCount;
    private int _rowCount;
    private VideoCellViewModel? _videoCellMaximized;
    
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
    
    public VideoCellViewModel? VideoCellMaximized
    {
        get => _videoCellMaximized;
        set => this.RaiseAndSetIfChanged(ref _videoCellMaximized, value);
    }
    
    public ObservableCollection<VideoCellViewModel> Items { get; } = [];
    public ReactiveCommand<string, Unit> OpenVideoPanelCommand { get; }
    public ReactiveCommand<VideoCellViewModel, Unit> MaximizeMinimizeCellCommand { get; }

    
    public MainVideoPanelViewModel()
    {
        OpenVideoPanelCommand = ReactiveCommand.Create<string>(OpenVideoPanel);
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<VideoCellViewModel>(MaximizeMinimizeCell);
        UpdateVideoPanel(4);
    }

    private void MaximizeMinimizeCell(VideoCellViewModel videoCell)
    {
        VideoCellMaximized = VideoCellMaximized == null ? videoCell : null;
        // TODO Нужно по хорошему остановить остальные ячейки.
    }

    private void OpenVideoPanel(string countCells)
    {
        switch (countCells)
        {
            case "one":
                UpdateVideoPanel(1);
                break;
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
        
        for (var i = currentCountCells; i < requiredCells; i++)
        {
            var videoCell = new VideoCellViewModel(i + 1);
            Items.Add(videoCell);
        }
        for (var i = currentCountCells; i > requiredCells; i--)
        {
            var videoCell = Items[i-1];
            Items.Remove(videoCell);
            videoCell.Dispose();
        }
    }

    public void Dispose()
    {
        while (Items.Count > 0)
        {
            var view = Items[0];
            view.Dispose();
            Items.Remove(view);
        }
    }
}