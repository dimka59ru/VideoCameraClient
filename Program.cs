using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;

namespace App;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Something very bad happened");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(GetLogFilePath("log.txt"),
                rollingInterval: RollingInterval.Month,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
    
    private static string GetLogFilePath(string fileName)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); 
        var companyName = Assembly.GetEntryAssembly()?.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault();
        return Path.Combine(
            appData, 
            companyName?.Company ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty, 
            "Logs", 
            fileName);
    }
}
