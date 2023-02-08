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

public class DataComboBox<TItem, TValue> : ComboBox<TValue>, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    [Parameter] public IList<TItem>? Items { get; set; }
    [Parameter] public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; } = default!;
    [Parameter] public EventCallback<TItem> SelectedItemChanged { get; set; } = default!;
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }
    [Parameter] public Func<TItem, string>? ItemText { get; set; }
    [Parameter] public Func<TItem, TValue>? ItemValue { get; set; }
    [Parameter] public string? DefaultItem { get; set; }

    protected override void OnParametersSet()
    {
        if (ChildContent == null)
        {
            ChildContent = RenderItems();
        }

        base.OnParametersSet();
    }

    internal override async Task<int> OnItemSelected(ComboBoxItem<TValue> item)
        => await SelectableChildContainerHelper.OnItemSelected<DataComboBox<TItem, TValue>, ComboBoxItem<TValue>, TItem>(this, item);

    private RenderFragment RenderItems()
    {
        return builder =>
        {
            int seq = 0;

            if (!string.IsNullOrEmpty(DefaultItem))
            {
                builder.OpenComponent<ComboBoxItem<TValue>>(seq++);
                builder.AddAttribute(seq++, nameof(ComboBoxItem<TValue>.Text), DefaultItem);
                builder.AddAttribute(seq++, nameof(ComboBoxItem<TValue>.Value), default(TValue));
                builder.CloseComponent();
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    builder.OpenComponent<ComboBoxItem<TValue>>(seq++);

                    builder.AddAttribute(seq++, nameof(ComboBoxItem<TValue>.Text), GetText(item));
                    builder.AddAttribute(seq++, nameof(ComboBoxItem<TValue>.Value), GetValue(item));

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
}
