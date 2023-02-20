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

                component.SelectedIndex = index;

                await component.SelectedIndexChanged.InvokeAsync(index);
                await component.SelectedIndicesChanged.InvokeAsync(component.SelectedIndices);
            }
            else if (component.Selection == SelectionMode.Multiple)
            {
                component.SelectedIndices.Remove(index);

                var lastIndex = -1;
                if (component.SelectedIndices.Any())
                {
                    lastIndex = component.SelectedIndices.LastOrDefault();
                    await component.SelectedIndexChanged.InvokeAsync(lastIndex);
                }
                else
                {
                    await component.SelectedIndexChanged.InvokeAsync(lastIndex);
                }
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
                var item = component.Items[index];

                await OnItemSelected(component, child, item);
            }
        }
        return index;
    }
    
    internal static async Task OnItemSelected<TComponent, TChild, TItem>(TComponent component, TChild child, TItem item)
        where TComponent : IDataBoundComponent<TItem>, IDataSelectionContainer<TItem>, ISelectionContainerComponent, IChildrenContainerComponent<TChild>
        where TChild : IStateChangeNotification
    {
        component.SelectedItem = item;
        await component.SelectedItemChanged.InvokeAsync(item);

        if (component.Items != null)
        {
            var selectedItems = component.Items.Where(x => component.SelectedIndices.Contains(component.Items.IndexOf(x))).ToList();

            component.SelectedItems = selectedItems;
            await component.SelectedItemsChanged.InvokeAsync(selectedItems);
        }
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

}
