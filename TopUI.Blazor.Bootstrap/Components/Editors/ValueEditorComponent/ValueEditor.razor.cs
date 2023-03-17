using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class ValueEditor<TValue>
{
    private bool IsNullable => Nullable.GetUnderlyingType(typeof(TValue)) != null;

    protected override string? FormatValueAsString(TValue? value)
    {
        return base.FormatValueAsString(value);
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        var targetType = typeof(TValue);
        if (targetType == typeof(string))
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentUICulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }

        result = default;
        validationErrorMessage = $"Object type ({typeof(TValue).Name}) not supported.";
        return false;
    }
}
