using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class AccordionPanel : IStateChangeNotification
{
    [CascadingParameter]
    [Browsable(false)]
    public Accordion Parent { get; set; } = default!;

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    [DefaultValue("Panel Title")]
    public string? Title { get; set; }


    protected override void OnInitialized()
    {
        if (Parent == null)
            throw new ArgumentNullException($"{nameof(AccordionPanel)} needs to be nested inside of a {nameof(Accordion)}.");

        (Parent as IChildrenContainerComponent<AccordionPanel>).AddChild(this);

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "accordion-item";
    }

    public override ValueTask DisposeAsync()
    {
        (Parent as IChildrenContainerComponent<AccordionPanel>).RemoveChild(this);

        return base.DisposeAsync();
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }
}
