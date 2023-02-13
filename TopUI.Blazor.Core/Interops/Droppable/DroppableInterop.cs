using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public sealed class DroppableInterop : IAsyncDisposable
{
    private IJSObjectReference _jsRef;
    private IDroppableHandler _handler;
    private DotNetObjectReference<DroppableInterop>? _objectReference;

    private DotNetObjectReference<DroppableInterop> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public DroppableInterop(IJSObjectReference jsRef, IDroppableHandler handler)
    {
        _jsRef = jsRef;
        _handler = handler;
    }

    public async ValueTask DisposeAsync()
    {
        await _jsRef.InvokeVoidAsync("dispose");
        await _jsRef.DisposeAsync();

        if (_objectReference != null)
            _objectReference.Dispose();
    }

    public async Task InitializeAsync(string id, Action<DroppableOptions> optionsBuilder)
    {
        var options = new DroppableOptions();
        optionsBuilder(options);

        await _jsRef.InvokeVoidAsync("initialize", ObjectReference, id, options);
    }
}
