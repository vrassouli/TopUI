using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Alert
{
    [Parameter] public bool Dismissable { get; set; }
    [Parameter] public string? Header { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public AlertModes Mode { get; set; } = AlertModes.Primary;

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "alert";
        yield return $"alert-{Mode}".ToLower();

        if (!string.IsNullOrEmpty(Icon))
        {
            yield return "d-flex";
            yield return "align-items-center";
        }

        if (Dismissable)
        {
            yield return "alert-dismissible";
            yield return "fade";
            yield return "show";
        }
    }
}
