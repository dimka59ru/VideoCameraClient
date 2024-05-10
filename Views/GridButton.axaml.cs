using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

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

    private readonly IBrush _initColor = new SolidColorBrush(Colors.Black);
    
    public GridButton()
    {
        InitializeComponent();
        var foundColor = Application.Current!.TryFindResource("BasicBlackBrush", this.ActualThemeVariant, out var result1);
        if (Application.Current!.TryFindResource("BasicBlackBrush", this.ActualThemeVariant, out var result)
            && result is IBrush brush)
            _initColor = brush;
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ColumnCountProperty)
        {
            InitGrid();
        }
        else if (change.Property == RowCountProperty)
        {
            InitGrid();
        }
    }

    private void InitGrid()
    {
        if (ColumnCount == 0 || RowCount == 0)
            return;
        
        for (var i = 0; i < RowCount; i++)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        }
        
        for (var j = 0; j < ColumnCount; j++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }
        
        for (var i = 0; i < RowCount; i++)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                var child = new Rectangle()
                {
                    Fill = _initColor,
                    Stroke = Brushes.Aqua,
                    Margin = new Thickness(1 ,1 ,0, 0)
                };
                
                MainGrid.Children.Add(child);
                Grid.SetRow(child, i);
                Grid.SetColumn(child, j);
            }
        }
    }
}