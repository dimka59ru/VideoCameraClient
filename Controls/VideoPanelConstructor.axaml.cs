using System.Linq;
using App.ViewModels;
using App.Views;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Controls;

public partial class VideoPanelConstructor : UserControl
{
    private int _columnCount;
    private int _rowCount;
    
    public VideoPanelConstructor()
    {
        InitializeComponent();
        
        MainGrid.RowDefinitions.Add(new RowDefinition());
        MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        var cell = new VideoCellView
        {
            DataContext = new VideoCellViewModel(1)
        };
        
        Grid.SetRow(cell, _rowCount);
        Grid.SetColumn(cell, _columnCount);
        
        MainGrid.Children.Add(cell);
    }
    
    private void ButtonAddRow_OnClick(object? sender, RoutedEventArgs e)
    {
        _rowCount++;
        MainGrid.RowDefinitions.Add(new RowDefinition());
        
        for (int i = 0; i <= _columnCount; i++)
        {
            var cell = new VideoCellView
            {
                DataContext = new VideoCellViewModel(0)
            };
            
            Grid.SetRow(cell, _rowCount);
            Grid.SetColumn(cell, i);
            MainGrid.Children.Add(cell);
        }
    }

    private void ButtonAddColumn_OnClick(object? sender, RoutedEventArgs e)
    {
        _columnCount++;
        MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
        
        for (int i = 0; i <= _rowCount; i++)
        {
            var cell = new VideoCellView
            {
                DataContext = new VideoCellViewModel(0)
            };
            
            Grid.SetRow(cell, i);
            Grid.SetColumn(cell, _columnCount);
            MainGrid.Children.Add(cell);
        }
    }

    private void ButtonDelRow_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_rowCount == 0)
            return;
        foreach ( var child in MainGrid.Children.ToArray() )
        {
            var childRow = (int)child.GetValue(Grid.RowProperty);
            if (childRow == _rowCount)
            {
                //this child should be removed
                MainGrid.Children.Remove(child); 
            }
        }
        
        MainGrid.RowDefinitions.RemoveAt(_rowCount);
        _rowCount--;
    }

    private void ButtonRemoveColumn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_columnCount == 0)
            return;
        
        foreach ( var child in MainGrid.Children.ToArray() )
        {
            var childColumn = (int)child.GetValue(Grid.ColumnProperty);
            if (childColumn == _columnCount)
            {
                //this child should be removed
                MainGrid.Children.Remove(child); 
            }
        }

        MainGrid.ColumnDefinitions.RemoveAt(_columnCount);
        _columnCount--;
    }
}