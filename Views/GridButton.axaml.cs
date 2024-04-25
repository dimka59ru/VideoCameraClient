using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace App.Views;

public partial class GridButton : UserControl
{
    private int _columnCount;
    public static readonly DirectProperty<GridButton, int> ColumnCountProperty = AvaloniaProperty.RegisterDirect<GridButton, int>(
        "ColumnCount", o => o.ColumnCount, (o, v) => o.ColumnCount = v);
    public int ColumnCount
    {
        get => _columnCount;
        set => SetAndRaise(ColumnCountProperty, ref _columnCount, value);
    }

    private int _rowCount;
    public static readonly DirectProperty<GridButton, int> RowCountProperty = AvaloniaProperty.RegisterDirect<GridButton, int>(
        "RowCount", o => o.RowCount, (o, v) => o.RowCount = v);
    public int RowCount
    {
        get => _rowCount;
        set => SetAndRaise(RowCountProperty, ref _rowCount, value);
    }
    
    public ObservableCollection<int> Items { get; } = [];
    
    public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register<GridButton, ICommand>(
        "Command");
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<GridButton, object>(
        "CommandParameter");
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    
    public GridButton()
    {
        InitializeComponent();
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ColumnCountProperty)
        {
            AddItems();
        }
        else if (change.Property == RowCountProperty)
        {
            AddItems();
        }
    }

    private void AddItems()
    {
        if (ColumnCount == 0 || RowCount == 0)
            return;
        for (var i = 0; i < ColumnCount * RowCount; i++)
        {
            Items.Add(i);
        }
    }
}