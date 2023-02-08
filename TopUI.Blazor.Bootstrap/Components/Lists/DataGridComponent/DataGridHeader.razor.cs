using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;

public sealed partial class DataGridHeader<TItem>
{
    //private DataGridColumn<TItem>? _orderByColumn;
    //private OrderByCommand<TItem>? _orderByCommand;

    [CascadingParameter] public DataGrid<TItem> DataGrid { get; set; } = default!;
    //[Parameter] public EventCallback<OrderByCommand?> OnOrderBy { get; set; }

    protected override void OnInitialized()
    {
        if (DataGrid == null)
            throw new ArgumentNullException($"{nameof(DataGridColumn<TItem>)} needs to be nested inside of a {nameof(DataGrid<TItem>)}.");

        base.OnInitialized();
    }

    //private void OnHeaderCellDown(DataGridColumn<TItem> column)
    //{
    //    _orderByColumn = column;

    //    if (_orderByCommand == null)
    //        _orderByCommand = new OrderByCommand<TItem> { Direction = SortDirection.Ascending, Expression = column.Field };
    //    else if (_orderByCommand.Direction == SortDirection.Ascending)
    //        _orderByCommand = new OrderByCommand<TItem> { Direction = SortDirection.Descending, Expression = column.Field };
    //    else
    //    {
    //        _orderByCommand = null;
    //        _orderByColumn = null;
    //    }

    //    OnOrderBy.InvokeAsync(_orderByCommand);
    //}
}
