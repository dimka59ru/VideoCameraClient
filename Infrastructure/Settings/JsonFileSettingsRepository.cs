using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace App.Infrastructure.Settings;

public abstract class JsonFileSettingsRepository<T> : ISettingsRepository<T> where T : new ()
{
    private static readonly object _lock = new object();
    
    private readonly string _filePath = GetLocalFilePath($"{typeof(T).Name}.json");
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };

    public T Load()
    {
        lock (_lock)
        {
            return File.Exists(_filePath)
                ? JsonSerializer.Deserialize<T>(File.ReadAllText(_filePath)) ?? new T()
                : new T();
        }
    }

    public void Save(T settings)
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(settings, _serializerOptions);
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            File.WriteAllText(_filePath, json);
        }
    }
    
    private static string GetLocalFilePath(string fileName)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); 
        var companyName = Assembly.GetEntryAssembly()?.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault();
        return Path.Combine(appData, companyName?.Company ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty, fileName);
    }
}