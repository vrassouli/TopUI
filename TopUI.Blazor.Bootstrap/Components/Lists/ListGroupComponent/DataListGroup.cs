using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class DataListGroup<TItem> : ListGroup, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    [Parameter] public IList<TItem>? Items { get; set; }
    [Parameter] public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; } = default!;
    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; } = default!;
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IList<TItem>?> SelectedItemsChanged { get; set; }
    [Parameter] public Func<TItem, string>? ItemText { get; set; }
    [Parameter] public Func<TItem, string?>? ItemIcon { get; set; }
    [Parameter] public Func<TItem, string?>? ItemUrl { get; set; }
    [Parameter] public Func<TItem, ListGroupItemMode>? ItemMode { get; set; }
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }

    protected override bool GenerateUl => ItemTemplate != null;

    protected override void OnParametersSet()
    {
        if (ChildContent == null)
        {
            ChildContent = RenderItems();
        }

        base.OnParametersSet();
    }

    internal override async Task<int> OnItemSelected(ListGroupItem item)
        => await SelectableChildContainerHelper.OnItemSelected<DataListGroup<TItem>, ListGroupItem, TItem>(this, item);

    private RenderFragment RenderItems()
    {
        return builder =>
        {
            int seq = 0;
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    builder.OpenComponent<ListGroupItem>(seq++);
                    if (ItemTemplate != null)
                    {
                        builder.AddAttribute(seq++, nameof(ListGroupItem.ChildContent), ItemTemplate.Invoke(item));
                    }
                    else
                    {
                        builder.AddAttribute(seq++, nameof(ListGroupItem.Text), GetText(item));
                        builder.AddAttribute(seq++, nameof(ListGroupItem.Url), GetUrl(item));
                        builder.AddAttribute(seq++, nameof(ListGroupItem.Icon), GetIcon(item));
                        builder.AddAttribute(seq++, nameof(ListGroupItem.Mode), GetMode(item));
                    }
                    builder.CloseComponent();
                }
            }
        };
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

    private string? GetUrl(TItem item)
    {
        if (ItemUrl != null)
            return ItemUrl(item);

        return null;
    }

    private ListGroupItemMode GetMode(TItem item)
    {
        if (ItemMode != null)
            return ItemMode(item);

        return ListGroupItemMode.Default;
    }
}
