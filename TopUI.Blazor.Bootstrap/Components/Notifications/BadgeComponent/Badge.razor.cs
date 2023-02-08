using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Badge
{
    [Parameter]
    [DefaultValue("+99")]
    public string? Message { get; set; }
    [Parameter] public BadgeModes Mode { get; set; } = BadgeModes.Danger;
    [Parameter] public BadgeType Type { get; set; } = BadgeType.Default;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (Type != BadgeType.Circle)
            yield return "badge";

        yield return $"text-bg-{Mode}".ToLower();

        if (Type == BadgeType.Pill)
            yield return "rounded-pill";
        else if (Type == BadgeType.Circle)
        {
            yield return "rounded-circle";
        }
    }
}
