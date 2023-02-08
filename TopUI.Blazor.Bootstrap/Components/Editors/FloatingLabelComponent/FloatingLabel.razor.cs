using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class FloatingLabel
{
    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            if (c != "form-label")
                yield return c;

        yield return "form-floating";
    }

}
