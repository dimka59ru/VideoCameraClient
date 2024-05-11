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
    private int _selectedCellIndex;
    private bool _isChannelSettingsOpened;
    private int _selectedPanelIndex;
    private ChannelSettingsViewModel? _channelSettings;

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
    
    public int SelectedCellIndex
    {
        get => _selectedCellIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedCellIndex, value);
    }
    
    public ChannelSettingsViewModel? ChannelSettings
    {
        get => _channelSettings;
        set => this.RaiseAndSetIfChanged(ref _channelSettings, value);
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
    public ReactiveCommand<int, Unit> OpenCloseChannelSettingsCommand { get; }

    
    public VideoPanelPageViewModel()
    {
        UserSettings.Load();
        SelectedPanelIndex = UserSettings.Instance.LastOpenPanelIndex;
        
        this.WhenAnyValue(x => x.SelectedPanelIndex)
            .Subscribe(OnSelectedPanelIndexChanged);
        
        this.WhenAnyValue(x => x.IsChannelSettingsOpened)
            .Subscribe(x => IsChannelSettingsOpenedChanged());
        
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<VideoCellViewModel>(MaximizeMinimizeCell);
        OpenCloseChannelSettingsCommand = ReactiveCommand.Create<int>(OpenCloseChannelSettings);
    }

    private void OnSelectedPanelIndexChanged(int index)
    {
        var selectedPanel = Panels[index];
        UpdateVideoPanel(selectedPanel);

        UserSettings.Instance.LastOpenPanelIndex = SelectedPanelIndex;
        UserSettings.Save();
    }
    
    private void IsChannelSettingsOpenedChanged()
    {
        if (IsChannelSettingsOpened)
        {
            ChannelSettings = new ChannelSettingsViewModel(SelectedCellIndex);
        }
        else
        {
            //Reinit Cell
            if (ChannelSettings is { SettingsChanged: true })
            {
                var oldCell = Cells[SelectedCellIndex];
                Cells[SelectedCellIndex] = new VideoCellViewModel(SelectedCellIndex);
                oldCell.Dispose();
            }
            
            ChannelSettings?.Dispose();
            ChannelSettings = null;
        }
    }
    
    private void OpenCloseChannelSettings(int cellIndex)
    {
        SelectedCellIndex = cellIndex;
        IsChannelSettingsOpened = !IsChannelSettingsOpened;
    }

    private void MaximizeMinimizeCell(VideoCellViewModel videoCell)
    {
        VideoCellMaximized = VideoCellMaximized == null ? videoCell : null;
        // TODO Нужно по хорошему остановить остальные ячейки.
    }

    private void UpdateVideoPanel(PanelParams value)
    {
        RowCount = value.RowCount;
        ColumnCount = value.ColumnCount;
        
        var requiredCells = RowCount * ColumnCount;
        var currentCountCells = Cells.Count;
        
        for (var i = currentCountCells; i < requiredCells; i++)
        {
            var videoCell = new VideoCellViewModel(i);
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