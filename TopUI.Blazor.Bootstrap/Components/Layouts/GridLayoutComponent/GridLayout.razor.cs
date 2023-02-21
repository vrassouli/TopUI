using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;
public partial class GridLayout
{
    [Parameter, ElementStyle("grid-template-columns")] public string? ColumnTemplates { get; set; }
    [Parameter] public int Columns { get; set; } = 1;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(ColumnTemplates))
            ColumnTemplates = GenerateColumnTemplates();

        base.OnInitialized();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-grid-layout";
    }

    private string GenerateColumnTemplates()
    {
        var fractions = new List<string>();

        for (int i = 0; i < Math.Max(1, Columns); i++)
        {
            fractions.Add("1fr");
        }

        return string.Join(' ', fractions);
    }
}
