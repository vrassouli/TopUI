using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class DataListGroup<TItem> : ListGroup, IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>
{
    private RenderFragment? _emptyTemplate;

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
    [Parameter] public RenderFragment? EmptyTemplate { get; set; }
    [Parameter] public string DefaultEmptyMessage { get; set; } = "List is empty.";

    [Inject] IStringLocalizer<Lists.ListGroupComponent.Resources.DataListGroup> Localizer { get; set; } = default!;

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
            if (Items?.Any() == true)
            {
                foreach (var item in Items)
                {
                    builder.OpenComponent<ListGroupItem>(0);
                    if (ItemTemplate != null)
                    {
                        builder.AddAttribute(1, nameof(ListGroupItem.ChildContent), ItemTemplate.Invoke(item));
                    }
                    else
                    {
                        builder.AddAttribute(2, nameof(ListGroupItem.Text), GetText(item));
                        builder.AddAttribute(3, nameof(ListGroupItem.Url), GetUrl(item));
                        builder.AddAttribute(4, nameof(ListGroupItem.Icon), GetIcon(item));
                        builder.AddAttribute(5, nameof(ListGroupItem.Mode), GetMode(item));
                    }
                    builder.CloseComponent();
                }
            }
            else
            {
                builder.AddContent(6, GetEmptyTemplate());
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

    private RenderFragment GetEmptyTemplate()
    {
        if (EmptyTemplate != null)
            return EmptyTemplate;

        return GetDefaultEmptyTemplate();
    }

    private RenderFragment GetDefaultEmptyTemplate()
    {
        if (_emptyTemplate == null)
            _emptyTemplate = builder =>
            {
                builder.OpenElement(0, "li");
                builder.AddAttribute(1, "class", "list-group-item text-center");
                
                // Icon
                builder.OpenElement(2, "i");
                builder.AddAttribute(3, "class", "bi bi-search me-2");
                builder.CloseElement();

                builder.AddContent(4, Localizer[DefaultEmptyMessage]);
                builder.CloseElement();
            };

        return _emptyTemplate;
    }

}
