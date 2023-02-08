using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class ListGroupItem : IStateChangeNotification
{
    [CascadingParameter] public ListGroup Parent { get; set; } = default!;
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string? Url { get; set; }
    [Parameter] public ListGroupItemMode Mode { get; set; } = ListGroupItemMode.Default;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool IsSelected => Parent.IsSelected(this);

    protected override void OnInitialized()
    {
        if (Parent == null)
            throw new ArgumentNullException($"{nameof(ListGroupItem)} needs to be nested inside of a {nameof(ListGroup)}.");

        (Parent as IChildrenContainerComponent<ListGroupItem>).AddChild(this);

        base.OnInitialized();
    }

    public override ValueTask DisposeAsync()
    {
        (Parent as IChildrenContainerComponent<ListGroupItem>).RemoveChild(this);

        return base.DisposeAsync();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "list-group-item";

        if (Parent.Selection != SelectionMode.None)
            yield return "list-group-item-action";

        if (IsSelected)
            yield return "active";

        if (Mode != ListGroupItemMode.Default)
            yield return $"list-group-item-{Mode}".ToLower();
    }

    private async Task OnPointerUp()
    {
        await Parent.OnItemSelected(this);
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }
}
