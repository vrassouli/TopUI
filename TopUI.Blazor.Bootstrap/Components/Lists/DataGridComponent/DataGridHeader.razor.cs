using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;

public sealed partial class DataGridHeader<TItem>
{
    [CascadingParameter] public DataGrid<TItem> DataGrid { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (DataGrid == null)
            throw new ArgumentNullException($"{nameof(DataGridColumn<TItem>)} needs to be nested inside of a {nameof(DataGrid<TItem>)}.");

        base.OnInitialized();
    }

    internal void OnStateChange()
    {
        StateHasChanged();
    }
}
