using App.Infrastructure.Settings;
using App.Models.Settings;

namespace Tests;

public class UserSettingsManagerTests
{
    [Fact]
    public void SaveLoad_Test()
    {
        var settingsRepository = new JsonFileSettingsRepository<UserSettings>();
        var usm = new UserSettingsManager(settingsRepository);

        var userSettings1 = usm.Load();
        userSettings1.LastOpenPanelIndex = 5;
        usm.Save(userSettings1);
        
        var userSettings2 = usm.Load();
        var lastOpenPanelIndex = userSettings2.LastOpenPanelIndex;
        
        Assert.Equal(5, lastOpenPanelIndex);
    }
}