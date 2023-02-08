using Microsoft.AspNetCore.Components;
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

public partial class ListGroup : IChildrenContainerComponent<ListGroupItem>, ISelectionContainerComponent
{
    [Parameter] public SelectionMode Selection { get; set; } = SelectionMode.Single;
    [Parameter] public bool Flush { get; set; }
    
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
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    public List<ListGroupItem> Children { get; } = new();

    public void AddChild(ListGroupItem child)
    {
        Children.Add(child);
    }

    public void RemoveChild(ListGroupItem child)
    {
        Children.Remove(child);
    }

    protected virtual bool GenerateUl => Selection == SelectionMode.None;

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "list-group";

        if (Flush)
            yield return "list-group-flush";
    }

    internal virtual async Task<int> OnItemSelected(ListGroupItem item)
        => await SelectableChildContainerHelper.OnItemSelected(this, item);

    internal bool IsSelected(ListGroupItem item)
        => SelectableChildContainerHelper.IsSelected(this, item);

}
