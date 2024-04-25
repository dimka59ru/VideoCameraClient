using System.Collections.ObjectModel;

namespace App.ViewModels;

public class PanelViewModel : ViewModelBase
{
    public ObservableCollection<int> Items { get; } = [];
}

public class FourCellsPanelViewModel : PanelViewModel
{
    public FourCellsPanelViewModel()
    {
        for (var i = 0; i < 4; i++)
        {
            Items.Add(i);
        }
    }
}

public class NineCellsPanelViewModel : PanelViewModel
{
    public NineCellsPanelViewModel()
    {
        for (var i = 0; i < 9; i++)
        {
            Items.Add(i);
        }
    }
}