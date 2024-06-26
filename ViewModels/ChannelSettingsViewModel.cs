using System;
using System.Windows.Input;
using App.Infrastructure.Settings;
using App.Models.Settings;
using ReactiveUI;


namespace App.ViewModels;

public class ChannelSettingsViewModel : ViewModelBase, IDisposable
{
    private readonly ISettingsRepository<UserSettings> _userSettingsManager;
    private readonly UserSettings _userSettings;
    private ChannelSettings _loadedChannelSettings;

    private string? _channelName;
    public string? ChannelName
    {
        get => _channelName;
        set => this.RaiseAndSetIfChanged(ref _channelName, value);
    }
    
    private string? _mainStreamUri;
    public string? MainStreamUri
    {
        get => _mainStreamUri;
        set => this.RaiseAndSetIfChanged(ref _mainStreamUri, value);
    }
    
    private bool _propertiesChanged;
    private bool PropertiesChanged
    {
        get => _propertiesChanged;
        set => this.RaiseAndSetIfChanged(ref _propertiesChanged, value);
    }
    
    private int _channelIndex;
    public int ChannelIndex
    {
        get => _channelIndex;
        set => this.RaiseAndSetIfChanged(ref _channelIndex, value);
    }

    private bool _settingsChanged;
    public bool SettingsChanged
    {
        get => _settingsChanged;
        private set => this.RaiseAndSetIfChanged(ref _settingsChanged, value);
    }

    public ICommand SaveSettingsCommand { get; }
    
    public ChannelSettingsViewModel(int channelIndex, ISettingsRepository<UserSettings> userSettingsManager)
    {
        _userSettingsManager = userSettingsManager ?? throw new ArgumentNullException(nameof(userSettingsManager));
        _userSettings = _userSettingsManager.Load();
            
        ChannelIndex = channelIndex;
        ChannelName = channelIndex.ToString();

        var canSaveSettingsCommandExecute = this.WhenAnyValue(
            x => x.ChannelName,
            x => x.MainStreamUri,
            x => x.PropertiesChanged,
            (channelName, mainStreamUri, settingsChanged) => 
                !string.IsNullOrEmpty(channelName)
                && (string.IsNullOrEmpty(mainStreamUri) || !string.IsNullOrEmpty(mainStreamUri) && Uri.TryCreate(mainStreamUri, UriKind.Absolute, out _))
                && settingsChanged);
        
        SaveSettingsCommand = ReactiveCommand.Create(OnSaveSettings, canSaveSettingsCommandExecute);
        
        var channelSettingsMap = _userSettings.ChannelSettingsMap;
        if (channelSettingsMap.TryGetValue(channelIndex, out var channelSettings))
        {
            ChannelName = string.IsNullOrEmpty(channelSettings.Name)
                ? ChannelName
                : channelSettings.Name;

            MainStreamUri = channelSettings.MainStreamUri?.ToString();
        }

        var streamUri = Uri.TryCreate(MainStreamUri, UriKind.Absolute, out var uri) ? uri : null;
        _loadedChannelSettings = new ChannelSettings(ChannelName, streamUri, null, null);
        
        this.WhenAnyValue(x => x.ChannelName,
                x => x.MainStreamUri)
            .Subscribe(x => CheckIfPropertiesChanged());
    }

    private void CheckIfPropertiesChanged()
    {
        var streamUri = Uri.TryCreate(MainStreamUri, UriKind.Absolute, out var uri) ? uri : null;
        var newSettings = new ChannelSettings(ChannelName, streamUri, null, null);

        PropertiesChanged = _loadedChannelSettings != newSettings;
    }

    private void OnSaveSettings()
    {
        var streamUri = Uri.TryCreate(MainStreamUri, UriKind.Absolute, out var uri) ? uri : null;
        var channelSettingsMap = _userSettings.ChannelSettingsMap;
        channelSettingsMap[ChannelIndex] = new ChannelSettings(ChannelName, streamUri, null, null);
        _loadedChannelSettings = channelSettingsMap[ChannelIndex];
        CheckIfPropertiesChanged();
        
        _userSettingsManager.Save(_userSettings);
        SettingsChanged = true;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}