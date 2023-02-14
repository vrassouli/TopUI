using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class BusyCover
{
    [Parameter] public string? BusyMessage { get; set; }
    [Parameter, ElementStyle("z-index")] public int? ZIndex { get; set; }
    [Parameter] public string Position { get; set; } = "absolute";

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-busy-cover";

        yield return $"position-{Position}";
    }
}
