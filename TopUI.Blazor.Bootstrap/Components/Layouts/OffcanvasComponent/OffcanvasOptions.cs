using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class OffcanvasOptions
{
    public string Title { get; set; }
    public bool StaticBackdrop { get; set; }
    public bool Keyboard { get; set; } = true;
    public bool Scrollable { get; set; } = true;
    public OffcanvasPlacement Placement { get; set; } = OffcanvasPlacement.End;
    public Dictionary<string, object>? Parameters { get; set; }

    public OffcanvasOptions(string title)
    {
        Title = title;
    }
}
