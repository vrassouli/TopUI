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

public partial class TreeView : IChildrenContainerComponent<TreeViewItem>, ISelectionContainerComponent, IStateChangeNotification
{
    [Parameter] public SelectionMode Selection { get; set; } = SelectionMode.Single;

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

    public List<TreeViewItem> Children { get; } = new();

    public void AddChild(TreeViewItem child)
    {
        Children.Add(child);
    }

    public void RemoveChild(TreeViewItem child)
    {
        Children.Remove(child);
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-tree";
    }

    internal virtual async Task<int> OnItemSelected(TreeViewItem item)
        => await SelectableChildContainerHelper.OnItemSelected(this, item);

    internal bool IsSelected(TreeViewItem item)
        => SelectableChildContainerHelper.IsSelected(this, item);

    public void OnStateChanged()
    {
        StateHasChanged();
    }
}
