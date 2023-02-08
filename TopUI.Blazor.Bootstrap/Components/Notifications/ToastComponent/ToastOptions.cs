using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class ToastOptions
{
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? Icon { get; set; }
    public bool CloseButton { get; set; }
    public int Delay { get; set; } = 5000;
    public bool AutoHide { get; set; } = true;

    public BackgroundColor? Background { get; set; }
    public TextColor? Color { get; set; }
}
