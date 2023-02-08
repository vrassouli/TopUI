using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class InputGroup
{
    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    [Browsable(false)]
    public RenderFragment? StartAddons { get; set; }

    [Parameter]
    [Browsable(false)]
    public RenderFragment? EndAddons { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "input-group";
    }
}
