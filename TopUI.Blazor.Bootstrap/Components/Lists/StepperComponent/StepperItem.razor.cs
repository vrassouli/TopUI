using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class StepperItem
{
    [CascadingParameter]
    [Browsable(false)]
    public Stepper Stepper { get; set; } = default!;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string? Description { get; set; }
    [Parameter] public StepperItemMode Mode { get; set; } = StepperItemMode.ToDo;

    protected override void OnInitialized()
    {
        if (Stepper == null)
            throw new InvalidOperationException($"{nameof(StepperItem)} must be nested inside a {nameof(Stepper)} component.");

        base.OnInitialized();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach(var c in base.GetClasses())
            yield return c;

        yield return "stepper-item";
        yield return $"{Mode}".ToLower();
    }
}
