using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Extensions;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class TextBox<TValue>
{
    [Parameter] public FormControlSize Size { get; set; } = FormControlSize.Default;

    [Parameter] public bool DisableAutoComplete { get; set; }
    [Parameter] public string? Format { get; set; }
    [Parameter] public string BindEventName { get; set; } = "onchange";

    [Parameter, ElementAttribute("placeholder")]
    public string? Placeholder { get; set; }

    [Parameter, ElementAttribute("rows")]
    public int? Lines { get; set; }


    protected override void OnParametersSet()
    {
        if (DisableAutoComplete)
            AddAttribute("role", "presentation");
        else
            RemoveAttribute("role", "presentation");

        if (Placeholder is null && ValueExpression?.GetPrompt() is not null)
            AddAttribute("placeholder", ValueExpression.GetPrompt()!);

        if (!AdditionalAttributes.ContainsKey("type") && GetInputType() != null)
            AddAttribute("type", GetInputType()!);

        base.OnParametersSet();
    }

    protected override string? FormatValueAsString(TValue? value)
    {
        if (value is Color color)
            return ColorTranslator.ToHtml(color);

        if (!string.IsNullOrEmpty(Format))
            return string.Format(Format, value);

        return base.FormatValueAsString(value);
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (EditorDataType == typeof(string) || IsNumeric)
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentUICulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }
        else if (EditorDataType == typeof(DateTime) || EditorDataType == typeof(DateOnly))
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentUICulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }
        else if (EditorDataType == typeof(Color))
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

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "form-control";
        if (EditorDataType == typeof(Color))
            yield return "form-control-color";

        if (Size != FormControlSize.Default)
            yield return $"form-control-{Size}".ToLower();
    }

    private string? GetInputType()
    {
        if (IsNumeric)
            return "number";
        else if (EditorDataType == typeof(DateTime))
            return "datetime";
        else if (EditorDataType == typeof(DateOnly))
            return "date";
        else if (EditorDataType == typeof(Color))
            return "color";
        else if (EditorDataType == typeof(string) && Lines == null)
            return "text";

        return null;
    }
}
