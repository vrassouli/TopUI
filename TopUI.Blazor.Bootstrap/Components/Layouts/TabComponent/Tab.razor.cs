using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Tab : IChildrenContainerComponent<TabPanel>
{
    private bool _preventRender = false;

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }


    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-tab";
    }

    public List<TabPanel> Children { get; } = new();

    public void AddChild(TabPanel child)
    {
        if (Children.Count == 0)
            child.Activate();

        Children.Add(child);

        StateHasChanged();
    }

    public void RemoveChild(TabPanel child)
    {
        var index = Children.IndexOf(child);

        if (index > -1)
        {
            Children.Remove(child);
            if (child.IsActive)
            {
                if (index == 0)
                    Children.FirstOrDefault()?.Activate();
                else
                    Children.ElementAt(index - 1).Activate();
            }
        }

        StateHasChanged();
    }

    protected override bool ShouldRender()
    {
        if (_preventRender)
        {
            _preventRender = false;
            return false;
        }

        return base.ShouldRender();
    }

    private void OnTabIndexChanged(int index)
    {
        /*
         * As we use bootstrap toggle attributes, to toggle tabs with bs javascript,
         * we don't need to re-render the component to reflect changes.
         * So we should prevent re-rendering of the component, but keep the state.
         * By keeping the state, we will have the same tab selected, if the parent component re-renders this tab component
         */

        _preventRender = true;
        Children.ForEach(c => c.Deactivate());
        Children.ElementAt(index).Activate();
    }
}
