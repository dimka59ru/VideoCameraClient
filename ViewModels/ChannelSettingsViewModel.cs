using System;
using System.Collections.Generic;
using System.Windows.Input;
using App.Models.Settings;
using ReactiveUI;


namespace App.ViewModels;

public class ChannelSettingsViewModel : ViewModelBase, IDisposable
{
    private readonly int _channelIndex;
    private readonly Dictionary<int, ChannelSettings> _channelSettingsMap;

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

    public ICommand SaveSettingsCommand { get; }
    
    public ChannelSettingsViewModel(int channelIndex)
    {
        _channelIndex = channelIndex;
        ChannelName = channelIndex.ToString();

        var canSaveSettingsCommandExecute = this.WhenAnyValue(
            x => x.ChannelName,
            x => x.MainStreamUri,
            (channelName, mainStreamUri) => 
                !string.IsNullOrEmpty(channelName) && 
                !string.IsNullOrEmpty(mainStreamUri) && Uri.TryCreate(mainStreamUri, UriKind.Absolute, out _));
        
        SaveSettingsCommand = ReactiveCommand.Create(OnSaveSettings, canSaveSettingsCommandExecute);
            
        UserSettings.Load();
        _channelSettingsMap = UserSettings.Instance.ChannelSettingsMap;
        
        if (_channelSettingsMap.TryGetValue(channelIndex, out var channelSettings))
        {
            ChannelName = string.IsNullOrEmpty(channelSettings.Name)
                ? ChannelName
                : channelSettings.Name;

            MainStreamUri = channelSettings.MainStreamUri?.ToString();
        }
    }

    private void OnSaveSettings()
    {
        _channelSettingsMap[_channelIndex] = new ChannelSettings
        {
            Name = ChannelName,
            MainStreamUri = Uri.TryCreate(MainStreamUri, UriKind.Absolute, out var streamUri) 
                ? streamUri 
                : null
        };
        
        UserSettings.Save();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}