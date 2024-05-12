namespace App.Infrastructure.Settings;

public interface ISettingsRepository<T> where T: new () 
{
    T Load();
    void Save(T settings);
}