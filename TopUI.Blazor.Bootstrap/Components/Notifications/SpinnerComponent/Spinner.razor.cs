using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Spinner
{
    [Parameter] public SpinnerType Type { get; set; } = SpinnerType.Border;
    [Parameter] public SpinnerMode Mode { get; set; } = SpinnerMode.Default;
    [Parameter] public SpinnerSize Size { get; set; } = SpinnerSize.Default;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (Type == SpinnerType.Border)
        {
            yield return "spinner-border";

            if (Size == SpinnerSize.Sm)
                yield return "spinner-border-sm";
        }
        else
        {
            yield return "spinner-grow";

            if (Size == SpinnerSize.Sm)
                yield return "spinner-grow-sm";
        }

        if (Mode != SpinnerMode.Default)
            yield return $"text-{Mode}".ToLower();
    }
}
