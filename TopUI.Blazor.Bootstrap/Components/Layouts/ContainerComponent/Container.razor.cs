using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Container
{
    [Parameter] public bool Fluid { get; set; }
    [Parameter] public Breakpoints Breakpoint { get; set; } = Breakpoints.Sm;

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (Fluid)
            yield return "container-fluid";
        else
            yield return $"container-{Breakpoint}".ToLower();
    }
}
