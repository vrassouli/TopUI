using Bootstrap.Blazor.Extensions;
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
    [Parameter] public Expression<Func<TItem, object?>>? FilterBy { get; set; }
    [Parameter] public Func<TItem, string?>? CellClass { get; set; }
    [Parameter] public RenderFragment<TItem>? ChildContent { get; set; }
    [Parameter] public string? Format { get; set; }
    [Parameter] public double? Width { get; set; }
    [Parameter] public bool AllowResize { get; set; } = true;
    [Parameter] public bool AllowFilter { get; set; } = true;

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

    internal string GetCellStyles()
    {
        if (Width != null)
            return $"width: {(Width ?? 0) + _deltaWidth}px";

        return $"flex: 1;";
    }

    internal object? GetData(TItem item)
    {
        if (_fieldFunc == null && Field != null)
            _fieldFunc = Field.Compile();

        if (_fieldFunc != null)
        {
            var value = _fieldFunc.Invoke(item);

            if (value is Enum enumVal)
                value = enumVal.GetDisplayName();

            if (!string.IsNullOrEmpty(Format))
                return string.Format(Format, value);

            return value;
        }

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

    internal Expression<Func<TItem, object?>>? GetOrderExpression()
    {
        return OrderBy ?? Field;
    }

    internal Expression<Func<TItem, object?>>? GetFilterExpression()
    {
        return FilterBy ?? Field;
    }

    //internal void OnStateChanged()
    //{
    //    StateHasChanged();
    //}
}
