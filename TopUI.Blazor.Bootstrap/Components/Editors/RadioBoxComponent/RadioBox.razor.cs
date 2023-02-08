using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class RadioBox<TValue>
{
    private string? _id = null;
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Inline { get; set; }
    [Parameter] public bool Reverse { get; set; }
    [Parameter] public string? GroupName { get; set; }
    [Parameter, EditorRequired] public TValue SelectionValue { get; set; } = default!;
    public override string Id => GetInputId();

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

    private string GetGroupName()
    {
        if (!string.IsNullOrEmpty(GroupName))
            return GroupName;

        if (ValueExpression != null)
            return ValueExpression?.GetMemberName() ?? string.Empty;

        return string.Empty;
    }

    private void OnChange(ChangeEventArgs args)
    {
        if (Equals(args.Value, "on"))
            CurrentValue = SelectionValue;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        throw new NotImplementedException();
    }

    private string GetInputId()
    {
        if (_id == null)
        {
            var memberName = ValueExpression?.GetMemberName();
            _id = $"{memberName}_{SelectionValue}";
        }

        if (_id == null)
            _id = base.Id;

        return _id;
    }
}
