using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core;

public abstract class UiComponentBase : ComponentBase
{
#if DEBUG
    private int _renderCounter = 0;
#endif

    protected override void OnAfterRender(bool firstRender)
    {
#if DEBUG
        Console.WriteLine($"{GetType().Name} rendered ({++_renderCounter})");
#endif
        base.OnAfterRender(firstRender);
    }
}
