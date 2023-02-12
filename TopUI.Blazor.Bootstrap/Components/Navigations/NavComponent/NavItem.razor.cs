using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class NavItem
{
    [CascadingParameter] public Nav Nav { get; set; } = default!;
    [CascadingParameter] public NavItem? ParentItem { get; set; }
    [Parameter] public string Title { get; set; } = "Item";
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string Url { get; set; } = "#";
    [Parameter] public string? LinkTarget { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if(Nav == null)
        {
            throw new ArgumentNullException($"{nameof(NavItem)} should be nested inside of a {nameof(Nav)} component");
        }

        base.OnInitialized();
    }
}
