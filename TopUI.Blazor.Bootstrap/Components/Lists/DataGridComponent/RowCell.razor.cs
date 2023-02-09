using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;

public sealed partial class RowCell<TItem> : IDisposable
{
    [Parameter, EditorRequired] public TItem Item { get; set; } = default!;
    [Parameter, EditorRequired] public DataGridColumn<TItem> Column { get; set; } = default!;
    [Parameter] public bool Placeholder { get; set; }

    protected override void OnInitialized()
    {
        if(Column == null)
            throw new ArgumentNullException(nameof(Column));

        Column.AddRowCell(this);

        base.OnInitialized();
    }

    public void Dispose()
    {
        Column.RemoveRowCell(this);
    }

    internal void OnHeaderStateChanged()
    {
        StateHasChanged();
    }
}
