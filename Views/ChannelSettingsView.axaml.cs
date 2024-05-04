using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace App.Views;

public partial class ChannelSettingsView : UserControl
{
    public static readonly StyledProperty<ICommand> CloseCommandProperty = 
        AvaloniaProperty.Register<ChannelSettingsView, ICommand>(nameof(CloseCommand));
    
    public ICommand CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }
    
    public ChannelSettingsView()
    {
        InitializeComponent();
    }
}