using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace App.Views;

public partial class VideoCellView : UserControl
{

    public static readonly StyledProperty<ICommand> MaximizeMinimizeCellCommandProperty = 
            AvaloniaProperty.Register<VideoCellView, ICommand>(nameof(MaximizeMinimizeCellCommand));

    public ICommand MaximizeMinimizeCellCommand
    {
        get => GetValue(MaximizeMinimizeCellCommandProperty);
        set => SetValue(MaximizeMinimizeCellCommandProperty, value);
    }

    public static readonly StyledProperty<bool> IsMaximizedProperty = AvaloniaProperty.Register<VideoCellView, bool>(
        nameof(IsMaximized));

    public bool IsMaximized
    {
        get => GetValue(IsMaximizedProperty);
        set => SetValue(IsMaximizedProperty, value);
    }
    
    public VideoCellView()
    {
        InitializeComponent();
    }
}