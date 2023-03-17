using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class RangeComponent<TValue>
{
    [Parameter, ElementAttribute("min")] public int? Min { get; set; }
    [Parameter, ElementAttribute("max")] public int? Max { get; set; }
    [Parameter, ElementAttribute("step")] public float? Step { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "form-range";
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (IsNumeric)
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentUICulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }

        result = default;
        validationErrorMessage = $"Data type '{EditorDataType.Name}' not supported.";
        return false;
    }
}
