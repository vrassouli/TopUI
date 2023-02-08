using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Abstractions;

public interface ISelectionContainerComponent
{
    SelectionMode Selection { get; set; }
    int SelectedIndex { get; set; }
    EventCallback<int> SelectedIndexChanged { get; set; }
    List<int> SelectedIndices { get; set; }
    EventCallback<IEnumerable<int>> SelectedIndicesChanged { get; set; }
}
