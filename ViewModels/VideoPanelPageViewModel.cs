using System;
using System.Collections.ObjectModel;
using System.Reactive;
using App.Infrastructure.Settings;
using App.Models;
using App.Models.Settings;
using ReactiveUI;

namespace App.ViewModels;

public class VideoPanelPageViewModel : ViewModelBase, IDisposable
{
    private readonly SettingsManager<UserSettings> _userSettingsManager;
    private readonly UserSettings _userSettings;
    
    private int _columnCount;
    public int ColumnCount
    {
        get => _columnCount;
        set => this.RaiseAndSetIfChanged(ref _columnCount, value);
    }
    
    private int _rowCount;
    public int RowCount
    {
        get => _rowCount;
        set => this.RaiseAndSetIfChanged(ref _rowCount, value);
    }
    
    private VideoCellViewModel? _videoCellMaximized;
    public VideoCellViewModel? VideoCellMaximized
    {
        get => _videoCellMaximized;
        set => this.RaiseAndSetIfChanged(ref _videoCellMaximized, value);
    }
    
    private int _selectedCellIndex;
    private int SelectedCellIndex
    {
        get => _selectedCellIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedCellIndex, value);
    }
    
    private ChannelSettingsViewModel? _channelSettings;
    public ChannelSettingsViewModel? ChannelSettings
    {
        get => _channelSettings;
        set => this.RaiseAndSetIfChanged(ref _channelSettings, value);
    }
    
    private bool _isChannelSettingsOpened;
    public bool IsChannelSettingsOpened
    {
        get => _isChannelSettingsOpened;
        set => this.RaiseAndSetIfChanged(ref _isChannelSettingsOpened, value);
    }
    
    private bool _isCellMaximized;
    public bool IsCellMaximized
    {
        get => _isCellMaximized;
        set => this.RaiseAndSetIfChanged(ref _isCellMaximized, value);
    }
    
    private int _selectedPanelIndex;
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
    
    public ReactiveCommand<int, Unit> MaximizeMinimizeCellCommand { get; }
    public ReactiveCommand<int, Unit> OpenCloseChannelSettingsCommand { get; }

    
    public VideoPanelPageViewModel(SettingsManager<UserSettings> userSettingsManager)
    {
        _userSettingsManager = userSettingsManager ?? throw new ArgumentNullException(nameof(userSettingsManager));
        _userSettings = _userSettingsManager.Load();
        
        SelectedPanelIndex = _userSettings.LastOpenPanelIndex;
        
        this.WhenAnyValue(x => x.SelectedPanelIndex)
            .Subscribe(OnSelectedPanelIndexChanged);
        
        this.WhenAnyValue(x => x.IsChannelSettingsOpened)
            .Subscribe(x => IsChannelSettingsOpenedChanged());
        
        this.WhenAnyValue(x => x.IsCellMaximized)
            .Subscribe(x => IsCellMaximizedChanged());
        
        MaximizeMinimizeCellCommand = ReactiveCommand.Create<int>(MaximizeMinimizeCell);
        OpenCloseChannelSettingsCommand = ReactiveCommand.Create<int>(OpenCloseChannelSettings);
    }

    private void IsCellMaximizedChanged()
    {
        if (IsCellMaximized)
        {
            VideoCellMaximized = Cells[SelectedCellIndex];
        }
    }

    private void OnSelectedPanelIndexChanged(int index)
    {
        var selectedPanel = Panels[index];
        UpdateVideoPanel(selectedPanel);

        _userSettings.LastOpenPanelIndex = SelectedPanelIndex;
        _userSettingsManager.Save(_userSettings);
    }
    
    private void IsChannelSettingsOpenedChanged()
    {
        if (IsChannelSettingsOpened)
        {
            ChannelSettings = new ChannelSettingsViewModel(SelectedCellIndex, _userSettingsManager);
        }
        else
        {
            //Reinit Cell
            if (ChannelSettings is { SettingsChanged: true })
            {
                var oldCell = Cells[SelectedCellIndex];
                Cells[SelectedCellIndex] = new VideoCellViewModel(SelectedCellIndex, _userSettingsManager);

                if (IsCellMaximized)
                {
                    VideoCellMaximized = Cells[SelectedCellIndex];
                }
                
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

    private void MaximizeMinimizeCell(int cellIndex)
    {
        SelectedCellIndex = cellIndex;
        IsCellMaximized = !IsCellMaximized;
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
            var videoCell = new VideoCellViewModel(i, _userSettingsManager);
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