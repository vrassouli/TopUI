using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class TabPanel : IStateChangeNotification
{
    [CascadingParameter]
    [Browsable(false)]
    public Tab Parent { get; set; } = default!;

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    [DefaultValue("Panel Title")]
    public string? Title { get; set; }

    internal bool IsActive { get; set; }


    protected override void OnInitialized()
    {
        if (Parent == null)
            throw new ArgumentNullException($"{nameof(TabPanel)} needs to be nested inside of a {nameof(Tab)}.");

        Parent.AddChild(this);

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
        Parent.RemoveChild(this);

        return base.DisposeAsync();
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }

    internal void Activate()
    {
        IsActive = true;
    }
    internal void Deactivate()
    {
        IsActive = false;
    }
}
