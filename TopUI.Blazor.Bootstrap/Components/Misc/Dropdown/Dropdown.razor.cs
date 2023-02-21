using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Dropdown
{
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter, EditorRequired] public string Text { get; set; } = string.Empty;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-dropdown";
    }
}
