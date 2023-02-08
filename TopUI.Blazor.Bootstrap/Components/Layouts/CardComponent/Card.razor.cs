using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Card
{
    [Parameter] 
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter] 
    public string? ImageSource { get; set; }

    [Parameter]
    [DefaultValue("Card Header")]
    public string? Header { get; set; }
    
    [Parameter] 
    [Browsable(false)]
    public RenderFragment? HeaderContent { get; set; }

    [Parameter] 
    public string? Footer { get; set; }

    [Parameter]
    [Browsable(false)]
    public RenderFragment? FooterContent { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "card";
    }
}
