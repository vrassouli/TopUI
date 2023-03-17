using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class CheckBox<TValue>
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Inline { get; set; }
    [Parameter] public bool Reverse { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "form-check";
        if (Inline)
            yield return "form-check-inline";
        if (Reverse)
            yield return "form-check-reverse";
    }

    private void OnChange(ChangeEventArgs args)
    {
        if (BindConverter.TryConvertTo<TValue>(Equals(args.Value, true), CultureInfo.CurrentUICulture, out var convertedVal))
            CurrentValue = convertedVal;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        throw new NotImplementedException();
    }
}
