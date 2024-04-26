namespace App.ViewModels;

public class VideoCellViewModel : ViewModelBase
{
    public int Index { get; }
    
    public VideoCellViewModel(int index)
    {
        Index = index;
    }
}