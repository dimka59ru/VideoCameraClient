using App.Infrastructure.Settings;
using App.Models.Settings;

namespace Tests;

public class UserSettingsManagerTests
{
    [Fact]
    public void SaveLoad_Test()
    {
        var expected = DateTime.UtcNow.DayOfYear;
        var usm = new UserSettingsManager();

        var userSettings1 = usm.Load();
        userSettings1.LastOpenPanelIndex = expected;
        usm.Save(userSettings1);
        
        var userSettings2 = usm.Load();
        var lastOpenPanelIndex = userSettings2.LastOpenPanelIndex;
        
        Assert.Equal(expected, lastOpenPanelIndex);
    }
}