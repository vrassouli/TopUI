using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Extensions;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public abstract class BootstrapFormControlComponent<TValue> : UiInputBase<TValue>
{
    [Parameter] public string? InputId { get; set; }

    private string? _id = null;
    public override string Id => GetInputId();

    private string GetInputId()
    {
        _id = InputId;

        if (_id == null)
            _id = ValueExpression?.GetMemberName();

        if (_id == null)
            _id = base.Id;

        return _id;
    }
}
