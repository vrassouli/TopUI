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

public sealed class DataDropdownButton<TItem> : DropdownButton, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    [Parameter] public IList<TItem>? Items { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; } = default!;
    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; } = default!;
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IList<TItem>?> SelectedItemsChanged { get; set; }
    [Parameter, EditorRequired] public Func<TItem, string> ItemTitle { get; set; } = default!;
    [Parameter] public Func<TItem, string?> ItemIcon { get; set; } = default!;
    [Parameter] public Func<TItem, bool> ItemIsActive { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (DropdownContent == null)
        {
            DropdownContent = RenderItems();
        }

        base.OnParametersSet();
    }

    internal override async Task<int> OnItemSelected(DropdownItem item)
        => await SelectableChildContainerHelper.OnItemSelected<DataDropdownButton<TItem>, DropdownItem, TItem>(this, item);

    private RenderFragment RenderItems()
    {
        return builder =>
        {
            int seq = 0;
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    builder.OpenComponent<DropdownItem>(seq++);
                    builder.AddAttribute(seq++, nameof(DropdownItem.Title), GetTitle(item));
                    builder.AddAttribute(seq++, nameof(DropdownItem.IsActive), GetIsActive(item));
                    builder.AddAttribute(seq++, nameof(DropdownItem.Icon), GetIcon(item));
                    builder.CloseComponent();
                }
            }
        };
    }

    private bool GetIsActive(TItem item)
    {
        if (ItemIsActive != null)
            return ItemIsActive(item);

        if (Equals(item, SelectedItem))
            return true;

        return false;
    }

    private string GetTitle(TItem item)
    {
        if (ItemTitle != null)
            return ItemTitle(item);

        return string.Empty;
    }

    private string? GetIcon(TItem item)
    {
        if (ItemIcon != null)
            return ItemIcon(item);

        return null;
    }

}
