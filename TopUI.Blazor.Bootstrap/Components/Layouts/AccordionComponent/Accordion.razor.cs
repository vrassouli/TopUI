using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Accordion : IChildrenContainerComponent<AccordionPanel>
{
    [Parameter] public bool Flush { get; set; }

    [Parameter]
    [DisplayName("Always Open")]
    public bool AlwaysOpen { get; set; }

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    public List<AccordionPanel> Children { get; } = new();

    public void AddChild(AccordionPanel child)
    {
        Children.Add(child);
    }

    public void RemoveChild(AccordionPanel child)
    {
        Children.Remove(child);
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "accordion";

        if (Flush)
            yield return "accordion-flush";
    }
}
