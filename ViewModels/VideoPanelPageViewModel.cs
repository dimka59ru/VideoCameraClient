using System;
using System.Collections.ObjectModel;
using System.Reactive;
using App.Models;
using App.Models.Settings;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPanelPageViewModel : ViewModelBase, IDisposable
{
    private int _columnCount;
    private int _rowCount;
    private VideoCellViewModel? _videoCellMaximized;
    private bool _isChannelSettingsOpened;
    private int _selectedPanelIndex;

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
    
    public int SelectedPanelIndex
    {
        get => _selectedPanelIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedPanelIndex, value);
    }
    
    public ObservableCollection<VideoCellViewModel> Cells { get; } = [];
    public ObservableCollection<PanelParams> Panels { get; } = 
    [
        new PanelParams(1, 1),
        new PanelParams(2, 1),
        new PanelParams(2, 2),
        new PanelParams(2, 3),
        new PanelParams(3, 3),
    ];
    
    public ReactiveCommand<VideoCellViewModel, Unit> MaximizeMinimizeCellCommand { get; }
    public ReactiveCommand<VideoCellViewModel, Unit> OpenCloseChannelSettingsCommand { get; }

    
    public VideoPanelPageViewModel()
    {
        UserSettings.Load();
        SelectedPanelIndex = UserSettings.Instance.LastOpenPanelIndex;
        
        this.WhenAnyValue(x => x.SelectedPanelIndex)
            .Subscribe(OnSelectedPanelIndexChanged);
        
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<VideoCellViewModel>(MaximizeMinimizeCell);
        OpenCloseChannelSettingsCommand = ReactiveCommand.Create<VideoCellViewModel>(OpenCloseChannelSettings);
    }

    private void OnSelectedPanelIndexChanged(int index)
    {
        var selectedPanel = Panels[index];
        UpdateVideoPanel(selectedPanel);

        UserSettings.Instance.LastOpenPanelIndex = SelectedPanelIndex;
        UserSettings.Save();
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
    private void UpdateVideoPanel(PanelParams value)
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