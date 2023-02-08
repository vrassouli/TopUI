using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;
using TopUI.Blazor.Bootstrap.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class DataGridColumn<TItem> : ComponentBase, IDisposable
{
    private double _deltaWidth;
    private List<RowCell<TItem>> _rowCells = new();
    private Func<TItem, object?>? _fieldFunc;

    [CascadingParameter] public DataGrid<TItem> DataGrid { get; set; } = default!;
    [Parameter] public string? Header { get; set; }
    [Parameter] public Expression<Func<TItem, object?>>? Field { get; set; }
    [Parameter] public Expression<Func<TItem, object?>>? OrderBy { get; set; }
    [Parameter] public RenderFragment<TItem>? ChildContent { get; set; }
    [Parameter] public double Width { get; set; } = 100;
    [Parameter] public bool AllowResize { get; set; } = true;

    protected override void OnInitialized()
    {
        if (DataGrid == null)
            throw new ArgumentNullException($"{nameof(DataGridColumn<TItem>)} needs to be nested inside of a {nameof(DataGrid<TItem>)}.");

        DataGrid.AddColumn(this);

        base.OnInitialized();
    }

    public void Dispose()
    {
        DataGrid.RemoveColumn(this);
    }

    internal string GetHeader()
    {
        return Header ?? Field?.GetDisplayName() ?? string.Empty;
    }

    internal double GetWidth()
    {
        return Width + _deltaWidth;
    }

    internal object? GetData(TItem item)
    {
        if (_fieldFunc == null && Field != null)
            _fieldFunc = Field.Compile();

        if (_fieldFunc != null)
            return _fieldFunc.Invoke(item);

        return null;
    }

    internal void UpdateWidth(double x)
    {
        _deltaWidth += x;

        foreach (var cell in _rowCells)
        {
            cell.OnHeaderStateChanged();
        }
    }

    internal void AddRowCell(RowCell<TItem> rowCell)
    {
        _rowCells.Add(rowCell);
    }

    internal void RemoveRowCell(RowCell<TItem> rowCell)
    {
        _rowCells.Remove(rowCell);
    }
}
