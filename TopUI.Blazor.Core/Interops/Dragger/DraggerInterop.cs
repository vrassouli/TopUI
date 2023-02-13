using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public class DraggerInterop : IAsyncDisposable
{
    private IJSObjectReference _jsRef;
    private readonly IDraggerHandler _handler;
    private DotNetObjectReference<DraggerInterop>? _objectReference;

    private DotNetObjectReference<DraggerInterop> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public DraggerInterop(IJSObjectReference jsRef, IDraggerHandler handler)
    {
        _jsRef = jsRef;
        _handler = handler;
    }

    public async Task InitializeAsync(string id, Action<DraggerOptions> optionsBuilder)
    {
        var options = new DraggerOptions();
        optionsBuilder(options);

        await _jsRef.InvokeVoidAsync("initialize", ObjectReference, id, options);
    }

    public async ValueTask DisposeAsync()
    {
        await _jsRef.InvokeVoidAsync("dispose");
        await _jsRef.DisposeAsync();

        if (_objectReference != null)
            _objectReference.Dispose();
    }

    [JSInvokable]
    public async Task OnDragging(DragMovment delta)
    {
        await _handler.Dragging(delta);
    }

    [JSInvokable]
    public async Task OnDragged(DragMovment delta)
    {
        await _handler.Dragged(delta);
    }
}
