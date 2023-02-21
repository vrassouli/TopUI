using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class DropdownItem : IStateChangeNotification
{
    [CascadingParameter] public DropdownButton Parent { get; set; } = default!;
    [Parameter] public string Title { get; set; } = "Item";
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool IsActive { get; set; }

    protected override void OnInitialized()
    {
        if (Parent == null)
            throw new ArgumentNullException($"{nameof(DropdownItem)} needs to be nested inside of a {nameof(DropdownButton)}.");

        (Parent as IChildrenContainerComponent<DropdownItem>).AddChild(this);

        base.OnInitialized();
    }

    public override ValueTask DisposeAsync()
    {
        (Parent as IChildrenContainerComponent<DropdownItem>).RemoveChild(this);

        return base.DisposeAsync();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "dropdown-item";

        if (IsActive)
            yield return "active";
    }

    private async Task OnPointerDown()
    {
        await Parent.OnItemSelected(this);
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }
}
