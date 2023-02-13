using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Interops;
using TopUI.Blazor.Core.Services.Abstraction;

namespace TopUI.Blazor.Core.Services;

internal class TopUiJs : ITopUiJs, IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public TopUiJs(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/TopUI.Blazor.Core/topui.interops.bundle.js").AsTask());
    }

    public async Task<DraggerInterop> GetDraggerAsync(IDraggerHandler handler)
    {
        var module = await _moduleTask.Value;
        var jsRef = await module.InvokeAsync<IJSObjectReference>("getDragger");

        return new DraggerInterop(jsRef, handler);
    }

    public async Task<DraggableInterop> GetDraggableAsync(IDraggableHandler handler)
    {
        var module = await _moduleTask.Value;
        var jsRef = await module.InvokeAsync<IJSObjectReference>("getDraggable");

        return new DraggableInterop(jsRef, handler);
    }

    public async Task<DroppableInterop> GetDroppableAsync(IDroppableHandler handler)
    {
        var module = await _moduleTask.Value;
        var jsRef = await module.InvokeAsync<IJSObjectReference>("getDrroppable");

        return new DroppableInterop(jsRef, handler);
    }

    public async Task<ScrollSyncInterop> GetScrollSyncAsync()
    {
        var module = await _moduleTask.Value;
        var jsRef = await module.InvokeAsync<IJSObjectReference>("getScrollSync");

        return new ScrollSyncInterop(jsRef);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
