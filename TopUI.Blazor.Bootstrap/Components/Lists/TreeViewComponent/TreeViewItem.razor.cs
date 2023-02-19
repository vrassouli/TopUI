using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class TreeViewItem : IChildrenContainerComponent<TreeViewItem>, IStateChangeNotification
{
    private bool _isExpanded;
    [CascadingParameter] public TreeView ParentTree { get; set; } = default!;
    [CascadingParameter] public TreeViewItem? ParentItem { get; set; }
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string? ExpandedIcon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Inject] IStringLocalizer<Lists.TreeViewComponent.Resources.TreeView> Localizer { get; set; } = default!;

    protected virtual string ExpanderIconClasses => "bi bi-chevron-right";
    protected virtual bool DisplayExpander => Children.Any();

    public List<TreeViewItem> Children { get; } = new();
    private bool IsSelected => ParentTree.IsSelected(this);
    private IEnumerable<TreeViewItem> Siblings => (ParentItem?.Children ?? ParentTree.Children).Where(x => x != this);

    public void AddChild(TreeViewItem child)
    {
        Children.Add(child);

        foreach (var sibling in Siblings)
            sibling.OnStateChanged();
    }

    public void RemoveChild(TreeViewItem child)
    {
        Children.Remove(child);

        foreach (var sibling in Siblings)
            sibling.OnStateChanged();
    }

    protected override void OnInitialized()
    {
        if (ParentTree == null)
            throw new ArgumentNullException($"{nameof(TreeViewItem)} needs to be nested inside of a {nameof(TreeView)}.");

        (ParentTree as IChildrenContainerComponent<TreeViewItem>).AddChild(this);
        (ParentItem as IChildrenContainerComponent<TreeViewItem>)?.AddChild(this);

        base.OnInitialized();
    }

    public override ValueTask DisposeAsync()
    {
        (ParentTree as IChildrenContainerComponent<TreeViewItem>).RemoveChild(this);
        (ParentItem as IChildrenContainerComponent<TreeViewItem>)?.RemoveChild(this);

        return base.DisposeAsync();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tree-item";

        if (_isExpanded)
            yield return "expanded";

        if (IsSelected)
            yield return "selected";
    }

    private async Task OnPointerUp()
    {
        await ParentTree.OnItemSelected(this);
    }

    protected virtual Task ToggleExpand()
    {
        _isExpanded = !_isExpanded;

        return Task.CompletedTask;
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }
}
