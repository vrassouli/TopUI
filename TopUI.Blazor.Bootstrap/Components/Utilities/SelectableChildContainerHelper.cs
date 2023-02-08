using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using TopUI.Blazor.Core.Abstractions;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components.Utilities;

internal class SelectableChildContainerHelper
{
    internal static async Task<int> OnItemSelected<TComponent, TChild>(TComponent component, TChild child)
        where TComponent : ISelectionContainerComponent, IChildrenContainerComponent<TChild>
        where TChild : IStateChangeNotification
    {
        if (component.Selection != SelectionMode.None)
        {
            var index = component.Children.IndexOf(child);

            if (!component.SelectedIndices.Contains(index))
            {
                if (component.Selection == SelectionMode.Single && component.SelectedIndices.Any())
                {
                    var prevIndex = component.SelectedIndices.FirstOrDefault();
                    component.SelectedIndices.Clear();

                    component.Children[prevIndex].OnStateChanged();
                }

                component.SelectedIndices.Add(index);

                await component.SelectedIndexChanged.InvokeAsync(index);
                await component.SelectedIndicesChanged.InvokeAsync(component.SelectedIndices);
            }
            else if (component.Selection == SelectionMode.Multiple)
            {
                component.SelectedIndices.Remove(index);

                if (component.SelectedIndices.Any())
                    await component.SelectedIndexChanged.InvokeAsync(component.SelectedIndices.LastOrDefault());
                else
                    await component.SelectedIndexChanged.InvokeAsync(-1);
                await component.SelectedIndicesChanged.InvokeAsync(component.SelectedIndices);
            }

            return index;
        }

        return -1;
    }

    internal static async Task<int> OnItemSelected<TComponent, TChild, TItem>(TComponent component, TChild child)
        where TComponent : IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>, ISelectionContainerComponent, IChildrenContainerComponent<TChild>
        where TChild : IStateChangeNotification
    {
        var index = await OnItemSelected<TComponent, TChild>(component, child);
        if (component.Items != null)
        {
            if (index >= 0 && index < component.Items.Count)
            {
                await component.SelectedItemChanged.InvokeAsync(component.Items[index]);

                var selectedItems = component.Items.Where(x => component.SelectedIndices.Contains(component.Items.IndexOf(x)));
                await component.SelectedItemsChanged.InvokeAsync(selectedItems);
            }
        }
        return index;
    }

    internal static bool IsSelected<TComponent, TChild>(TComponent component, TChild item)
        where TComponent : ISelectionContainerComponent, IChildrenContainerComponent<TChild>
        where TChild : IStateChangeNotification
    {
        var index = component.Children.IndexOf(item);
        if (index > -1)
            return component.SelectedIndices.Contains(index);

        return false;
    }

    //private static void NotifyItemsStateChange<TComponent, TChild>(TComponent component, List<int> selectedIndicies)
    //    where TComponent : ISelectionContainerComponent, IChildrenContainerComponent<TChild>
    //    where TChild : IStateChangeNotification
    //{
    //    foreach (var index in selectedIndicies)
    //    {
    //        var child = component.Children[index];
    //        child.OnStateChanged();
    //    }
    //}

}
