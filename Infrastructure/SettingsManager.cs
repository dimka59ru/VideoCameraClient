using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;


namespace App.Infrastructure;

public abstract class SettingsManager<T> where T : SettingsManager<T>, new()
{
    private static readonly string _filePath = GetLocalFilePath($"{typeof(T).Name}.json");
    public static T Instance { get; private set; }

    private static string GetLocalFilePath(string fileName)
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); 
        var companyName = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault();
        return Path.Combine(appData, companyName?.Company ?? Assembly.GetEntryAssembly().GetName().Name, fileName);
    }

    public static void Load() =>
        Instance = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<T>(File.ReadAllText(_filePath)) 
            : new T();

    public static void Save()
    {
        string json = JsonSerializer.Serialize(Instance);
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        File.WriteAllText(_filePath, json);
    }
}