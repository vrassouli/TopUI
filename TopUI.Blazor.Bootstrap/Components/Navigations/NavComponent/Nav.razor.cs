using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Nav
{
    [Parameter] public bool Vertical { get; set; }
    [Parameter] public bool Pills { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "nav";
        yield return "tui-nav";
        if (Vertical)
            yield return "flex-column";
        if (Pills)
            yield return "nav-pills";
    }
}
