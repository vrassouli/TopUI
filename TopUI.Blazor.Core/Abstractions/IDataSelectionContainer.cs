using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Abstractions;

public interface IDataSelectionContainer<TItem>
{
    TItem? SelectedItem { get; set; }
    EventCallback<TItem> SelectedItemChanged { get; set; }
    IList<TItem>? SelectedItems { get; set; }
    EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }
}
