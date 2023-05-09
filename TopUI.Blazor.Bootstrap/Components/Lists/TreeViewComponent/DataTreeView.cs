using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Lists.TreeViewComponent;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public class DataTreeView<TItem> : TreeView, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{

    [Parameter] public IList<TItem>? Items { get; set; }
    [Parameter] public TreeViewItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; } = default!;
    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; } = default!;
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IList<TItem>?> SelectedItemsChanged { get; set; }

    [Parameter] public Func<TItem, string>? ItemText { get; set; }
    [Parameter] public Func<TItem, string?>? ItemIcon { get; set; }
    [Parameter] public Func<TItem, string?>? ItemExpandedIcon { get; set; }
    [Parameter] public Func<TItem, bool>? ItemHasChildren { get; set; }
    [Parameter] public Func<TItem, IEnumerable<TItem>?>? ItemSubItems { get; set; }
    [Parameter] public Func<TItem, object>? ItemKey { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        //if (ChildContent == null)
        //{
            ChildContent = RenderItems(await GetRootItems());
        //}
        await base.OnParametersSetAsync();
    }

    private async Task<IList<TItem>?> GetRootItems()
    {
        if (Items != null)
            return Items;

        if (ItemsProvider != null)
            return await ItemsProvider.Invoke(default);

        return null;
    }

    internal override async Task<int> OnItemSelected(TreeViewItem item)
    {
        var index = await SelectableChildContainerHelper.OnItemSelected<DataTreeView<TItem>, TreeViewItem>(this, item);

        if (item is DataTreeViewItem<TItem> dataTreeViewItem)
            await SelectableChildContainerHelper.OnItemSelected<DataTreeView<TItem>, TreeViewItem, TItem>(this, item, dataTreeViewItem.Item);

        return index;
    }


    internal RenderFragment? RenderItems(IEnumerable<TItem>? items)
    {
        if (items?.Any() == true)
        {
            return builder =>
            {
                foreach (var item in items)
                {
                    builder.OpenComponent<DataTreeViewItem<TItem>>(0);
                    //builder.SetKey(GetKey(item) ?? item);
                    builder.AddAttribute(1, nameof(DataTreeViewItem<TItem>.Item), item);
                    builder.AddAttribute(2, nameof(DataTreeViewItem<TItem>.ItemsProvider), ItemsProvider);

                    builder.AddAttribute(3, nameof(DataTreeViewItem<TItem>.Text), GetText(item));
                    builder.AddAttribute(4, nameof(DataTreeViewItem<TItem>.Icon), GetIcon(item));
                    builder.AddAttribute(5, nameof(DataTreeViewItem<TItem>.ExpandedIcon), GetExpandedIcon(item));
                    builder.AddAttribute(5, nameof(DataTreeViewItem<TItem>.HasChildren), GetHasChildren(item));

                    //builder.AddAttribute(3, nameof(DataTreeViewItem<TItem>.ItemText), ItemText);
                    //builder.AddAttribute(4, nameof(DataTreeViewItem<TItem>.ItemIcon), ItemIcon);
                    //builder.AddAttribute(5, nameof(DataTreeViewItem<TItem>.ItemExpandedIcon), ItemExpandedIcon);
                    //builder.AddAttribute(6, nameof(DataTreeViewItem<TItem>.ItemSubItems), ItemSubItems);
                    //builder.AddAttribute(7, nameof(DataTreeViewItem<TItem>.ItemHasChildren), ItemHasChildren);

                    builder.CloseComponent();
                }
            };
        }

        return null;
    }

    private string GetText(TItem item)
    {
        if (ItemText != null)
            return ItemText(item);

        return string.Empty;
    }

    private string? GetIcon(TItem item)
    {
        if (ItemIcon != null)
            return ItemIcon(item);

        return null;
    }

    private string? GetExpandedIcon(TItem item)
    {
        if (ItemExpandedIcon != null)
            return ItemExpandedIcon(item);

        return null;
    }

    private bool GetHasChildren(TItem item)
    {
        if (ItemHasChildren != null)
            return ItemHasChildren(item);

        return false;
    }

    internal IEnumerable<TItem>? GetSubItems(TItem item)
    {
        if (ItemSubItems != null)
            return ItemSubItems(item);

        return null;
    }

    private object? GetKey(TItem item)
    {
        if (ItemKey != null)
            return ItemKey(item);

        return item ?? default;
    }
}
