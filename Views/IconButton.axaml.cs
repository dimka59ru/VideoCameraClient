using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace App.Views;

public class IconButton : TemplatedControl
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<IconButton, ICommand>(nameof(IconButton));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<IconButton, string>(
        nameof(Text), "");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<Geometry> PathIconProperty = AvaloniaProperty.Register<IconButton, Geometry>(
        nameof(PathIcon));

    public Geometry PathIcon
    {
        get => GetValue(PathIconProperty);
        set => SetValue(PathIconProperty, value);
    }

    public static readonly StyledProperty<double> IconWidthProperty = AvaloniaProperty.Register<IconButton, double>(
        "IconWidth");

    public double IconWidth
    {
        get => GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }

    public static readonly StyledProperty<double> IconHeightProperty = AvaloniaProperty.Register<IconButton, double>(
        "IconHeight");

    public double IconHeight
    {
        get => GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }
}