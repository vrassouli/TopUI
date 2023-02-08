using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using Bootstrap.Blazor.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class StackPanel
{
    [Parameter] public StackDirection Direction { get; set; } = StackDirection.Vertical;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (Direction == StackDirection.Horizontal)
            yield return "hstack";
        else
            yield return "vstack";
    }

}
