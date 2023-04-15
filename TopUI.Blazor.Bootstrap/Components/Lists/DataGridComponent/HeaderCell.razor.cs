using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Interops;
using TopUI.Blazor.Core.Services.Abstraction;

namespace TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;

public sealed partial class HeaderCell<TItem> : IAsyncDisposable, IDraggerHandler
{
    string? _resizerId;
    string? _filterId;
    private DraggerInterop? _dragger;

    [CascadingParameter] public DataGrid<TItem> DataGrid { get; set; } = default!;
    [Parameter, EditorRequired] public DataGridColumn<TItem> Column { get; set; } = default!;

    [Inject] private ITopUiJs TopUi { get; set; } = default!;
    private SortDirection SortDirection => DataGrid.GetSortDirection(Column);
    private bool IsFiltered => DataGrid.IsFiltered(Column);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Column.AllowResize)
            await InitializeResizer();

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task InitializeResizer()
    {
        if (string.IsNullOrEmpty(_resizerId))
            return;

        _dragger = await TopUi.GetDraggerAsync(this);

        await _dragger.InitializeAsync(_resizerId, opt =>
        {
            opt.AllowVertical = false;
        });
    }

    private string GetResizerId()
    {
        if (string.IsNullOrEmpty(_resizerId))
            _resizerId = $"_{Guid.NewGuid()}".Replace('-', '_');

        return _resizerId;
    }

    private async Task OnCellClicked()
    {
        await DataGrid.OnColumnClicked(Column);
    }

    private void OnFilterClicked()
    {
        DataGrid.OnFilterClicked(Column);
    }

    public async ValueTask DisposeAsync()
    {
        if (_dragger != null)
            await _dragger.DisposeAsync();
    }

    private string GetFilterIcon()
    {
        if (IsFiltered)
            return "bi-funnel-fill";

        return "bi-funnel";
    }

    #region IDragHandler

    public Task Dragged(DragMovment delta)
    {
        Column.UpdateWidth(delta.Dx);
        StateHasChanged();

        return Task.CompletedTask;
    }

    public Task Dragging(DragMovment delta)
    {
        return Task.CompletedTask;
    }

    #endregion
}
