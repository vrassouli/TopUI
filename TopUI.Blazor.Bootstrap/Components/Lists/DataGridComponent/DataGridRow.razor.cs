using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;

public sealed partial class DataGridRow<TItem>
{
    [Parameter, EditorRequired] public TItem Item { get; set; } = default!;
    [CascadingParameter] public DataGrid<TItem> DataGrid { get; set; } = default!;

    private bool IsSelected => DataGrid.IsSelected(Item);

    protected override void OnInitialized()
    {
        if (DataGrid == null)
            throw new ArgumentNullException($"{nameof(DataGridRow<TItem>)} needs to be nested inside of a {nameof(DataGrid<TItem>)}.");

        DataGrid.AddRow(this);

        base.OnInitialized();
    }

    public void Dispose()
    {
        DataGrid.RemoveRow(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine("After render");

        base.OnAfterRender(firstRender);
    }

    private async Task OnRowClick()
    {
        await DataGrid.OnItemSelected(Item);
    }

    internal void Refresh()
    {
        StateHasChanged();
    }
}
