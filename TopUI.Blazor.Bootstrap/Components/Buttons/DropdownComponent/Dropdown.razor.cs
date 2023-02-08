﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Utilities;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Dropdown : IChildrenContainerComponent<DropdownItem>, ISelectionContainerComponent
{
    [Parameter] public SelectionMode Selection { get; set; } = SelectionMode.Single;
    
    [Parameter]
    [Browsable(false)]
    public virtual RenderFragment? DropdownContent { get; set; }

    [Parameter] 
    [Browsable(false)]
    public int SelectedIndex { get; set; }

    [Parameter]
    [Browsable(false)]
    public EventCallback<int> SelectedIndexChanged { get; set; }

    [Parameter] 
    [Browsable(false)]
    public List<int> SelectedIndices { get; set; } = new();

    [Parameter]
    [Browsable(false)]
    public EventCallback<IEnumerable<int>> SelectedIndicesChanged { get; set; }
    [Parameter] 
    public bool SplitButton { get; set; }

    public List<DropdownItem> Children { get; } = new();

    public void AddChild(DropdownItem child)
    {
        Children.Add(child);
    }

    public void RemoveChild(DropdownItem child)
    {
        Children.Remove(child);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (!SplitButton)
        {
            AddAttribute("data-bs-toggle", "dropdown");
            AddAttribute("aria-expanded", "false");
        }
        else
        {
            RemoveAttribute("data-bs-toggle", "dropdown");
            RemoveAttribute("aria-expanded");
        }
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (!SplitButton)
        {
            yield return "dropdown-toggle";
        }
    }

    internal virtual async Task<int> OnItemSelected(DropdownItem item)
        => await SelectableChildContainerHelper.OnItemSelected(this, item);

    internal bool IsSelected(DropdownItem item)
        => SelectableChildContainerHelper.IsSelected(this, item);
}
