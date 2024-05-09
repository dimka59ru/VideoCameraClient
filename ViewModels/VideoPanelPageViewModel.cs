using System;
using System.Collections.ObjectModel;
using System.Reactive;
using App.Models;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPanelPageViewModel : ViewModelBase, IDisposable
{
    private int _columnCount;
    private int _rowCount;
    private VideoCellViewModel? _videoCellMaximized;
    private bool _isChannelSettingsOpened;
    private PanelButtonParams? _selectedPanelButton;

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
    
    public PanelButtonParams? SelectedPanelButton
    {
        get => _selectedPanelButton;
        set => this.RaiseAndSetIfChanged(ref _selectedPanelButton, value);
    }
    
    public ObservableCollection<VideoCellViewModel> Cells { get; } = [];
    public ObservableCollection<PanelButtonParams> PanelButtons { get; } = 
    [
        new PanelButtonParams(1, 1),
        new PanelButtonParams(2, 1),
        new PanelButtonParams(2, 2),
        new PanelButtonParams(2, 3),
        new PanelButtonParams(3, 3),
    ];
    
    public ReactiveCommand<VideoCellViewModel, Unit> MaximizeMinimizeCellCommand { get; }
    public ReactiveCommand<VideoCellViewModel, Unit> OpenCloseChannelSettingsCommand { get; }

    
    public VideoPanelPageViewModel()
    {
        SelectedPanelButton = PanelButtons[2];
        this.WhenAnyValue(x => x.SelectedPanelButton)
            .Subscribe(OnSelectedPanelButtonChanged);
        
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<VideoCellViewModel>(MaximizeMinimizeCell);
        OpenCloseChannelSettingsCommand = ReactiveCommand.Create<VideoCellViewModel>(OpenCloseChannelSettings);
        
    }

    private void OnSelectedPanelButtonChanged(PanelButtonParams? value)
    {
        if (value is null) return;
        UpdateVideoPanel(value);
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
    private void UpdateVideoPanel(PanelButtonParams value)
    {
        RowCount = value.RowCount;
        ColumnCount = value.ColumnCount;
        
        var requiredCells = RowCount * ColumnCount;
        var currentCountCells = Cells.Count;
        
        for (var i = currentCountCells; i < requiredCells; i++)
        {
            var videoCell = new VideoCellViewModel(i + 1);
            Cells.Add(videoCell);
        }
        for (var i = currentCountCells; i > requiredCells; i--)
        {
            var videoCell = Cells[i-1];
            Cells.Remove(videoCell);
            videoCell.Dispose();
        }
    }

    public void Dispose()
    {
        while (Cells.Count > 0)
        {
            var view = Cells[0];
            view.Dispose();
            Cells.Remove(view);
        }
    }
}