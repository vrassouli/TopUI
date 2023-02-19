using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class MasterDetail
{
    [Parameter] public RenderFragment? Master { get; set; }
    [Parameter] public RenderFragment? Detail { get; set; }
    [Parameter] public Breakpoints Breakpoint { get; set; } = Breakpoints.Md;
    [Parameter] public bool FillMaster { get; set; }
    [Parameter] public string? MasterClasses { get; set; }
    [Parameter] public string? DetailClasses { get; set; }
    [Parameter] public string? DetailTitle { get; set; }
    [Parameter] public EventCallback OnCloseDetail { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;
        yield return "tui-master-detail";
        yield return $"tui-master-detail-{Breakpoint}".ToLower();
    }

    protected IEnumerable<string> GetMasterClasses()
    {
        yield return "master";

        if (FillMaster)
            yield return "flex-fill";

        if (!string.IsNullOrEmpty(MasterClasses))
            yield return MasterClasses;
    }

    protected IEnumerable<string> GetDetailClasses()
    {
        yield return "detail";

        if (!FillMaster)
            yield return "flex-fill";

        if (!string.IsNullOrEmpty(DetailClasses))
            yield return DetailClasses;
    }
}
