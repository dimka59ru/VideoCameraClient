using System;

namespace App.Infrastructure.Settings;

public abstract class SettingsManager<T> where T : new ()
{
    private readonly ISettingsRepository<T> _settingsRepository;

    protected SettingsManager(ISettingsRepository<T> settingsRepository)
    {
        _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
    }
    
    public T Load()
    {
        return _settingsRepository.Load();
    }
    
    public void Save(T settings)
    {
        _settingsRepository.Save(settings);
    }
}