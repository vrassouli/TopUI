using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class DialogOptions
{
    public string Title { get; set; }
    public bool StaticBackdrop { get; set; }
    public bool Keyboard { get; set; } = true;
    public bool Scrollable { get; set; } = true;
    public bool Centered{ get; set; }
    public DialogSize Size { get; set; } = DialogSize.Default;
    public DialogFullScreenBreakpoint FullScreen { get; set; } = DialogFullScreenBreakpoint.Never;
    public Dictionary<string, object>? Parameters { get; set; }

    public DialogOptions(string title)
    {
        Title = title;
    }
}
