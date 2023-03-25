using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Stepper
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public StepperDirection Direction { get; set; } = StepperDirection.Horizontal;
    [Parameter] public string? DoneIcon { get; set; }
    [Parameter] public string? ActiveIcon { get; set; }
    [Parameter] public string? TodoIcon { get; set; }
    [Parameter] public bool ActiveDescription { get; set; } = false;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-stepper";
        yield return $"{Direction}".ToLower();
        if (ActiveDescription)
            yield return "active-description";
    }
}
