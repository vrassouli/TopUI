using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class ComboBoxItem<TValue> : IStateChangeNotification, IAsyncDisposable
{
    [CascadingParameter] public ComboBox<TValue> Parent { get; set; } = default!;
    [Parameter, EditorRequired] public string Text { get; set; } = default!;
    [Parameter, EditorRequired] public string Value { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Parent == null)
            throw new ArgumentNullException($"{nameof(ComboBoxItem<TValue>)} needs to be nested inside of a {nameof(ComboBox<TValue>)}.");

        (Parent as IChildrenContainerComponent<ComboBoxItem<TValue>>).AddChild(this);

        base.OnInitialized();
    }

    public ValueTask DisposeAsync()
    {
        (Parent as IChildrenContainerComponent<ComboBoxItem<TValue>>).RemoveChild(this);
        
        return ValueTask.CompletedTask;
    }

    public void OnStateChanged()
    {
        StateHasChanged();
    }

    //public string? GetValueString()
    //{
    //    return Parent.ValueToString(Value);
    //}

    public bool IsSelected()
    {
        return Parent.IsSelected(this);
    }
}