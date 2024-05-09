namespace App.Models;

public class PanelParams
{
    public PanelParams(int rowCount, int columnCount)
    {
        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
}