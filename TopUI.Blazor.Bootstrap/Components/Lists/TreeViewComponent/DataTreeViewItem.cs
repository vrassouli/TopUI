using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Lists.TreeViewComponent;

internal class DataTreeViewItem<TItem> : TreeViewItem
{
    private IEnumerable<TItem>? _items;
    private bool _loading;

    [Parameter] public TItem Item { get; set; } = default!;
    [Parameter] public TreeViewItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
    [Parameter] public bool HasChildren { get; set; }

    private DataTreeView<TItem>? ParentDataTree => ParentTree as DataTreeView<TItem>;

    protected override string ExpanderIconClasses => _loading ? "bi bi-arrow-clockwise rotate" : base.ExpanderIconClasses;
    protected override bool DisplayExpander => ParentDataTree?.GetSubItems(Item)?.Any() ?? ParentDataTree?.GetHasChildren(Item) ?? false;

    protected override void OnInitialized()
    {
        if (Item is null)
            throw new ArgumentNullException(nameof(Item));

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (ChildContent == null)
        {
            if (_items?.Any() != true)
                _items = ParentDataTree?.GetSubItems(Item);

            ChildContent = ParentDataTree?.RenderItems(_items);
        }

        base.OnParametersSet();
    }

    protected override async Task ToggleExpand()
    {
        try
        {
            if (_items?.Any() != true &&
                ParentDataTree?.GetHasChildren(Item) == true &&
                ItemsProvider != null)
            {
                _loading = true;

                _items = await ItemsProvider.Invoke(Item);
                ChildContent = ParentDataTree?.RenderItems(_items);
            }

            await base.ToggleExpand();
        }
        finally
        {
            _loading = false;
        }
    }
}
