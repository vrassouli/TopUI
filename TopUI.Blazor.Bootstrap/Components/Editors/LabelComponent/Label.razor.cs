using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Label
{
    private string? _text;
    private string? _inputId;

    [Parameter] public string? Text { get; set; }
    [Parameter] public string? InputId { get; set; }
    [Parameter] public Expression<Func<object>>? For { get; set; }

    protected override void OnParametersSet()
    {
        GetText();
        GetInputId();

        base.OnParametersSet();
    }

    private void GetText()
    {
        if (!string.IsNullOrEmpty(Text))
            _text = Text;

        if (For != null)
            _text = For.GetDisplayName();
    }

    private void GetInputId()
    {
        if (!string.IsNullOrEmpty(InputId))
            _inputId = Text;

        if (For != null)
            _inputId = For.GetMemberName();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "form-label";
    }
}
