using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Lists.DataGridComponent;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;
using TopUI.Blazor.Core.Interops;
using TopUI.Blazor.Core.Services.Abstraction;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class DataGrid<TItem> : IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    private ScrollSync? _scrollSync;
    private DataGridColumn<TItem>? _orderedColumn;
    private SortDirection _sortDirection = SortDirection.None;
    private Virtualize<TItem>? _itemsContainer;
    private string? _searchText;

    [Parameter] public EventCallback<string?> OnSearch { get; set; }
    [Parameter] public RenderFragment? Toolbar { get; set; }

    [Parameter]
    [Browsable(false)]
    public IList<TItem>? Items { get; set; }

    [Parameter]
    [Browsable(false)]
    public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }

    [Parameter, EditorRequired]
    [Browsable(false)]
    public RenderFragment Columns { get; set; } = default!;

    [Parameter]
    [Browsable(false)]
    public EventCallback<OrderByCommand?> OnOrderBy { get; set; }

    #region Selection

    [Parameter]
    [Browsable(false)]
    public TItem? SelectedItem { get; set; }

    [Parameter]
    [Browsable(false)]
    public EventCallback<TItem?> SelectedItemChanged { get; set; }

    [Parameter]
    [Browsable(false)]
    public IList<TItem>? SelectedItems { get; set; }

    [Parameter]
    [Browsable(false)]
    public EventCallback<IList<TItem>> SelectedItemsChanged { get; set; }

    [Parameter] public SelectionMode Selection { get; set; } = SelectionMode.Single;

    #endregion

    [Inject] private ITopUiJs TopUi { get; set; } = default!;
    [Inject] private IStringLocalizer<Lists.DataGridComponent.Resources.DataGrid> Localizer { get; set; } = default!;

    internal List<DataGridColumn<TItem>> DataColumns { get; set; } = new();
    internal List<DataGridRow<TItem>> DataRows { get; set; } = new();

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-data-grid";
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SyncScrollbars();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public async Task RefreshAsync()
    {
        if (_itemsContainer != null)
            await _itemsContainer.RefreshDataAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        if (_scrollSync != null)
            await _scrollSync.DisposeAsync();

        await base.DisposeAsync();
    }

    internal async Task OnItemSelected(TItem item)
    {
        if (Selection == SelectionMode.None)
            return;

        if (SelectedItems == null)
            SelectedItems = new List<TItem>();


        if (SelectedItems.Contains(item))
        {
            SelectedItems.Remove(item);
        }
        else
        {
            if (Selection == SelectionMode.Single)
                ClearSelections();

            SelectedItems.Add(item);
        }

        SelectedItem = SelectedItems.LastOrDefault();

        await SelectedItemChanged.InvokeAsync(SelectedItem);
        await SelectedItemsChanged.InvokeAsync(SelectedItems);
    }

    private void ClearSelections()
    {
        if (SelectedItems?.Any() != true)
            return;

        var list = SelectedItems.ToList();
        SelectedItems.Clear();

        foreach (var item in list)
        {
            var row = DataRows.FirstOrDefault(x => Equals(x.Item, item));
            if (row != null)
                row.Refresh();
        }
    }

    internal bool IsSelected(TItem item)
    {
        return SelectedItems?.Contains(item) ?? false;
    }

    internal void AddColumn(DataGridColumn<TItem> column)
    {
        DataColumns.Add(column);
    }

    internal void RemoveColumn(DataGridColumn<TItem> column)
    {
        DataColumns.Remove(column);
    }

    internal void AddRow(DataGridRow<TItem> row)
    {
        DataRows.Add(row);
    }

    internal void RemoveRow(DataGridRow<TItem> row)
    {
        DataRows.Remove(row);
    }

    internal async Task OnColumnClicked(DataGridColumn<TItem> column)
    {
        _orderedColumn = column;

        if (_sortDirection == SortDirection.None)
            _sortDirection = SortDirection.Ascending;
        else if (_sortDirection == SortDirection.Ascending)
            _sortDirection = SortDirection.Descending;
        else
            _sortDirection = SortDirection.None;

        await OnOrderBy.InvokeAsync(new OrderByCommand<TItem>
        {
            Direction = _sortDirection,
            Expression = _orderedColumn.OrderBy ?? _orderedColumn.Field
        });

        await RefreshAsync();
    }

    internal SortDirection GetSortDirection(DataGridColumn<TItem> column)
    {
        if (column == _orderedColumn)
            return _sortDirection;

        return SortDirection.None;
    }

    private async Task SyncScrollbars()
    {
        _scrollSync = await TopUi.GetScrollSyncAsync();

        await _scrollSync.InitializeAsync($"#{Id}>.data-grid-content", new List<string> { $"#{Id}>.data-grid-header" }, opt =>
        {
            opt.SyncVertical = false;
        });
    }

    private async ValueTask<ItemsProviderResult<TItem>> GetItemsProvider(ItemsProviderRequest request)
    {
        if (Items == null && ItemsProvider != null)
        {
            return await ItemsProvider.Invoke(request);
        }
        else if (Items != null && ItemsProvider == null)
        {
            var list = Items.Skip(request.StartIndex).Take(request.Count);
            return new ItemsProviderResult<TItem>(list, Items.Count);
        }

        return new ItemsProviderResult<TItem>(Enumerable.Empty<TItem>(), 0);
    }

    private async Task OnSearchChanged()
    {
        await OnSearch.InvokeAsync(_searchText);
    }
}
