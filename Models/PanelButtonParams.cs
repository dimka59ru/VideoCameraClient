namespace App.Models;

public class PanelButtonParams
{
    public PanelButtonParams(int rowCount, int columnCount)
    {
        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
}