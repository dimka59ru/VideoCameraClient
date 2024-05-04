using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPanelPageViewModel : ViewModelBase, IDisposable
{
    private int _columnCount;
    private int _rowCount;
    private VideoCellViewModel? _videoCellMaximized;
    private bool _isChannelSettingsOpened;
    
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
    
    public bool IsChannelSettingsOpened
    {
        get => _isChannelSettingsOpened;
        set => this.RaiseAndSetIfChanged(ref _isChannelSettingsOpened, value);
    }
    
    public ObservableCollection<VideoCellViewModel> Items { get; } = [];
    public ReactiveCommand<List<int>, Unit> OpenVideoPanelCommand { get; }
    public ReactiveCommand<VideoCellViewModel, Unit> MaximizeMinimizeCellCommand { get; }
    public ReactiveCommand<VideoCellViewModel, Unit> OpenCloseChannelSettingsCommand { get; }

    
    public VideoPanelPageViewModel()
    {
        OpenVideoPanelCommand = ReactiveCommand.Create<List<int>>(UpdateVideoPanel);
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<VideoCellViewModel>(MaximizeMinimizeCell);
        OpenCloseChannelSettingsCommand = ReactiveCommand.Create<VideoCellViewModel>(OpenCloseChannelSettings);
        UpdateVideoPanel([2, 2]);
    }

    private void OpenCloseChannelSettings(VideoCellViewModel obj)
    {
        IsChannelSettingsOpened = !IsChannelSettingsOpened;
    }

    private void MaximizeMinimizeCell(VideoCellViewModel videoCell)
    {
        VideoCellMaximized = VideoCellMaximized == null ? videoCell : null;
        // TODO Нужно по хорошему остановить остальные ячейки.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="panelParams">first: rows, second: columns</param>
    private void UpdateVideoPanel(List<int> panelParams)
    {
        if (panelParams.Count != 2 )
            throw new ArgumentException("incorrect number of parameters");
        
        RowCount = panelParams[0];
        ColumnCount = panelParams[1];
        
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