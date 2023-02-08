using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Drawer
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Sidebar { get; set; }
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public Breakpoints Breakpoint { get; set; } = Breakpoints.Lg;
    [Parameter] public bool FluidContainer { get; set; }
    [Parameter] public string? SidebarHeader { get; set; }
    [Parameter] public bool SidebarCloseButton { get; set; }

    #region Css Classes

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-drawer";
        yield return $"tui-drawer-{Breakpoint}".ToLower();
        yield return "navbar";
        yield return $"navbar-expand-{Breakpoint}".ToLower();
    }

    private string GetNavbarContainerClass()
    {
        return $"container-{Breakpoint}".ToLower();
    }

    private string GetLayoutContainerClass()
    {
        if (FluidContainer)
            return "tui-drawer-container container-fluid";

        return $"tui-drawer-container container-{Breakpoint}".ToLower();
    }

    private string GetSidebarOffcanvasClass()
    {
        return $"offcanvas-{Breakpoint} offcanvas-start".ToLower();
    }

    #endregion
}
