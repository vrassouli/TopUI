using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Editors.ComboBoxComponent;

public sealed class EnumComboBox<TValue> : DataComboBox<TValue, TValue>
{
    private Type? _enumType = null;

    //[Parameter, EditorRequired] public Type EnumType { get; set; } = default!;

    private bool IsNullable => Nullable.GetUnderlyingType(typeof(TValue)) != null;
    private Type DataType => Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

    protected override void OnParametersSet()
    {
        if (_enumType != typeof(TValue))
            Reload();

        _enumType = typeof(TValue);

        base.OnParametersSet();
    }

    private void Reload()
    {
        var list = new List<TValue>();

        foreach (TValue item in Enum.GetValues(DataType))
            list.Add(item);

        Items = list;

        ItemValue = x => x;
        ItemText = x => x.ToString() ?? "Select...";
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (Enum.TryParse(DataType, value, out var enumResult))
        {
            if (enumResult is TValue val)
            {
                result = val;
                validationErrorMessage = null;

                return true;
            }
        }
        else if (IsNullable)
        {
            result = default!;
            validationErrorMessage = null;
            return true;
        }

        result = default!;
        validationErrorMessage = "Invalid value is provided.";
        return false;
    }
}
