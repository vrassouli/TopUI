using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Progress
{
    [Parameter] public double Value { get; set; }
    [Parameter] public bool DisplayLabel { get; set; }
    [Parameter] public ProgressMode Mode { get; set; } = ProgressMode.Default;
    [Parameter] public ProgressType Type { get; set; } = ProgressType.Default;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "progress";
    }

    private string GetProgressClasses()
    {
        var list = new List<string>();

        if (Mode != ProgressMode.Default)
            list.Add($"bg-{Mode}".ToLower());

        if (Type == ProgressType.Stripped)
        {
            list.Add("progress-bar-striped");
        }
        else if (Type == ProgressType.AnimatedStrips)
        {
            list.Add("progress-bar-striped");
            list.Add("progress-bar-animated");
        }
        return string.Join(" ", list);
    }
}
