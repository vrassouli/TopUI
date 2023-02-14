using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Dashboard
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach(var c in base.GetClasses())
            yield return c;

        yield return "tui-dashboard";
    }
}
