using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public class DataComboBox<TItem, TValue> : ComboBox<TValue>, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    [Parameter] public IList<TItem>? Items { get; set; }
    [Parameter] public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; } = default!;
    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; } = default!;
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IList<TItem>?> SelectedItemsChanged { get; set; }
    [Parameter] public Func<TItem, string>? ItemText { get; set; }
    [Parameter] public Func<TItem, TValue>? ItemValue { get; set; }
    [Parameter] public Func<TItem, object>? ItemKey { get; set; }
    [Parameter] public string? DefaultItem { get; set; }

    protected override void OnParametersSet()
    {
        if (ChildContent == null)
        {
            ChildContent = RenderItems();
        }

        base.OnParametersSet();
    }

    internal override async Task<int> OnItemSelected(ComboBoxItem<TValue> child)
    {
        //await SelectableChildContainerHelper.OnItemSelected<DataComboBox<TItem, TValue>, ComboBoxItem<TValue>, TItem>(this, item);
        var index = await SelectableChildContainerHelper.OnItemSelected<DataComboBox<TItem, TValue>, ComboBoxItem<TValue>>(this, child);
        if (Items != null)
        {
            // DefaultItem adds an extra Item (<option>) to the <select> element.
            // so the real index of this data items, are added by 1, at the runtime
            var itemIndex = index - (string.IsNullOrEmpty(DefaultItem) ? 0 : 1);
            if (itemIndex >= 0 && itemIndex < Items.Count)
            {
                var item = Items[itemIndex];

                await SelectableChildContainerHelper.OnItemSelected(this, child, item);
            }
            else
            {
                SelectedItem = default;
                SelectedItems?.Clear();

                await SelectedItemChanged.InvokeAsync(SelectedItem);
                await SelectedItemsChanged.InvokeAsync(SelectedItems);

            }
        }
        return index;

    }

    private RenderFragment RenderItems()
    {
        return builder =>
        {
            if (!string.IsNullOrEmpty(DefaultItem))
            {
                builder.OpenComponent<ComboBoxItem<TValue>>(0);
                builder.AddAttribute(1, nameof(ComboBoxItem<TValue>.Text), DefaultItem);
                builder.AddAttribute(2, nameof(ComboBoxItem<TValue>.Value), default(TValue));
                builder.CloseComponent();
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    builder.OpenComponent<ComboBoxItem<TValue>>(0);
                    builder.SetKey(GetKey(item) ?? item);

                    builder.AddAttribute(1, nameof(ComboBoxItem<TValue>.Text), GetText(item));
                    builder.AddAttribute(2, nameof(ComboBoxItem<TValue>.Value), GetValue(item));

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

    private TValue? GetValue(TItem item)
    {
        if (ItemValue != null)
            return ItemValue(item);

        return default;
    }

    private object? GetKey(TItem item)
    {
        if (ItemKey != null)
            return ItemKey(item);

        return GetValue(item) ?? default;
    }
}
